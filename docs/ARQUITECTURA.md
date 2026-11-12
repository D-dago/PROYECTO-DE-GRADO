# Arquitectura

## Frontend

React, TypeScript, Vite y CSS. Las rutas se resuelven en `App.tsx` (`/`, `/registro`, `/login`, `/perfil`, `/test`, `/recomendaciones`, `/historial`, `/skins`, `/admin`) y consumen la API mediante servicios. El token JWT se envía en el encabezado `Authorization`.

Al compilar (`npm run build`), la web se publica dentro de `wwwroot` del backend: en producción la API sirve la página y todo queda en un solo puerto y un solo enlace.

## Backend

ASP.NET Core Web API organizado en cuatro proyectos:

- `API`: controladores, autenticación, autorización y HTTP.
- `Application`: contratos, interfaces y casos de uso.
- `Domain`: entidades, constantes, estados y banco de preguntas.
- `Infrastructure`: EF Core, PostgreSQL, servicios y migraciones.

## PostgreSQL

`OrientacionVocacionalDbContext` usa Npgsql. Las migraciones crean usuarios, perfiles, preguntas, intentos, preguntas asignadas, respuestas, perfiles vocacionales, instituciones, carreras, compatibilidades, historial y skins.

## JWT

El backend emite JWT con identificador de usuario, correo y rol. Los controladores obtienen el usuario desde `ClaimTypes.NameIdentifier`; no confían en un UserId enviado por React. También existe inicio de sesión con Google (`POST /api/auth/google`), que valida el `idToken` contra Google y emite el mismo JWT propio.

## Test RIASEC

El test aplica el **Perfil de Intereses O*NET** en su versión oficial en español: 30 actividades de trabajo, cinco por cada una de las seis dimensiones de Holland: `R` (Realista), `I` (Investigador), `A` (Artístico), `S` (Social), `E` (Emprendedor) y `C` (Convencional). La migración `TestPerfilInteresesOnet` las cargó (códigos `OR1` a `OC5`) y dejó inactivas (orden ≥ 1000) las 60 preguntas propias anteriores; las de CHASIDE (áreas H y D) ya lo estaban. Se conservan porque hay respuestas guardadas que las referencian. Los textos son de las O*NET Career Exploration Tools (USDOL/ETA) y se usan sin cambios, según la O*NET Tools Developer License.

Es una **forma fija**: todos los intentos reciben las mismas 30 actividades (las 5 primeras activas de cada dimensión), en el orden oficial `R, I, A, S, E, C, R, I, A, S, E, C, …`, de modo que nunca aparezcan dos seguidas de la misma dimensión. Antes se sorteaban 4 de 10 por dimensión, y así cada estudiante respondía un test distinto. La pantalla no muestra la dimensión de cada actividad. Se responde con la escala de O*NET (Me disgusta mucho … Me gusta mucho, guardada como 1 a 5) y cada respuesta se guarda al instante.

El algoritmo `ONET-30-V4` puntúa cada respuesta de 0 a 4 (respuesta − 1) y suma cada dimensión (máximo 20 = 5 × 4). El código del perfil sigue a O*NET: solo cuentan las dimensiones por encima del promedio de las seis del propio estudiante (puntaje z > 0), hasta tres, de mayor a menor; los empates de la primera se resuelven en el orden `R, I, A, S, E, C` y los demás por cercanía a la primera en el hexágono. Si ninguna supera el promedio, el perfil es plano: se guarda sin código y la interpretación lo explica.

Los resultados de los tests anteriores de 24 preguntas (`RIASEC-24-V3`, valores 1 a 5, máximo 20) y de 12 preguntas (`RIASEC-12-V2`, máximo 10) se siguen mostrando y generan recomendaciones con su propia puntuación: cada perfil se mide contra el máximo de su propio test, así una persona que obtuvo 10 de 10 tiene la misma compatibilidad que una que obtiene 20 de 20.

Los perfiles de versiones anteriores se conservan pero no se interpretan: sus letras significan otra cosa, así que no generan recomendaciones ni lectura de perfil.

## Motor de reglas

Las compatibilidades se almacenan por carrera y dimensión RIASEC. Las reglas que se habían cargado con letras CHASIDE se convirtieron a su dimensión RIASEC más cercana (H→S, I→R, D→R, E→I; C, A y S sin cambio). El servidor combina puntuaciones, dimensión principal/secundaria y nivel de regla para calcular un porcentaje reproducible y una explicación textual. La IA no interviene en este cálculo.

## Catálogo

Instituciones y carreras se almacenan con estado, fuente y fecha de actualización. No se siembran instituciones ni carreras ficticias. El listado público muestra solo las activas; el administrador tiene su propio listado que incluye las inactivas para poder reactivarlas.

## Datos en vivo

Cada pantalla abierta consulta `GET /api/live/version` cada 5 segundos y recarga sus datos solo si la versión cambió. El contador vive en memoria (`DataVersion`), no consulta la base y queda fuera del límite anti-abuso. Las peticiones que modifican datos lo avanzan con `[NotifyDataChange]`. Si se agrega otra acción que cambie datos del catálogo fuera de esos controladores, debe llevar ese atributo.

## Filtros y recomendaciones

La interfaz del estudiante ya no usa estos filtros: muestra las instituciones compatibles con su enlace oficial (ver `docs/decisiones.md`). El backend los conserva porque están probados y no estorban.

El backend aplica primero los filtros traducibles a SQL (institución, costo y duración máximos) y luego los de texto libre (municipio, modalidad, nivel y área) en memoria, ignorando mayúsculas y tildes: así "sacaba" encuentra "Sacaba" e "informatico" encuentra "Informático". El filtro de área busca además en el nombre de la carrera.

Cada consulta se registra en `recommendation_history`. El historial que ve el estudiante muestra **una entrada por test terminado** —no una por búsqueda—, con su perfil, puntajes, interpretación y los filtros de la última búsqueda hecha con ese resultado.

## Monedas y skins

Cada respuesta otorga 1 moneda y terminar el test otorga 50. Las monedas se gastan en un catálogo fijo de skins (ropa del avatar) definido en código, no en datos institucionales.

## IA opcional

`IAiExplanationService` recibe únicamente el perfil vocacional y recomendaciones estructuradas. Usa `AI__ProviderUrl`, `AI__Model` y `AI__ApiKey` desde configuración del backend. Sin clave o modelo devuelve un mensaje de indisponibilidad y no bloquea recomendaciones.

## Administrador

Las rutas `/api/admin/*` exigen `Authorize(Roles = "ADMINISTRADOR")`. Desde `/admin` se gestionan dashboard, instituciones, carreras, compatibilidades, preguntas del test y estudiantes, y se consultan los resultados con su interpretación, exportables a Excel (con el detalle de respuestas) y PDF. Las contraseñas nunca se devuelven.

Una pregunta no se puede desactivar ni mover de dimensión si deja a su dimensión con menos de 2 preguntas activas: sin ellas el test no se podría armar.

## Seguridad operativa

- Límite de 100 KB por petición y límites de frecuencia por IP (registro, login y global).
- Registro cerrado a correos desechables, alias con `+` y a partir de 5000 cuentas.
- Cabeceras `X-Content-Type-Options`, `X-Frame-Options` y `Referrer-Policy`.
- Manejador global de excepciones: el cliente recibe siempre un JSON genérico y el detalle queda en el log del servidor.
- `GET /health` comprueba que la API responde y alcanza la base de datos.
- Swagger se publica únicamente en entorno de desarrollo.
