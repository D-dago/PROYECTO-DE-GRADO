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

El banco contiene 60 preguntas activas, diez por cada una de las seis dimensiones: `R` (Realista), `I` (Investigador), `A` (Artístico), `S` (Social), `E` (Emprendedor) y `C` (Convencional), según la teoría de Holland. Durante un tiempo el sistema aplicó una versión CHASIDE del test; la migración `VuelveATestRiasec` restauró el banco RIASEC y dejó inactivas (orden ≥ 1000) las preguntas propias de CHASIDE (áreas H y D), porque hay respuestas guardadas que las referencian.

Cada intento presenta **las mismas 24 preguntas a todos los estudiantes** (las 4 primeras de cada dimensión del banco de 60, sin sorteo) y las intercala `R, I, A, S, E, C, R, I, A, S, E, C, …`, de modo que nunca aparezcan dos preguntas seguidas de la misma dimensión. Todas siguen la forma «¿Qué tanto te interesa…?» con un ejemplo debajo (banco corregido según la revisión de Gabriela Lino). Se responden en una escala de 1 a 5, sin límite de tiempo, y cada respuesta se guarda al instante. La pantalla no muestra la dimensión de cada pregunta mientras se responde.

El algoritmo suma las seis dimensiones, aplica un desempate fijo en el orden `R, I, A, S, E, C` y guarda el perfil con versión `RIASEC-24-V5`. El puntaje máximo por dimensión es 20 (4 preguntas × 5). Los resultados de versiones anteriores (`RIASEC-60-V4`, máximo 50; `RIASEC-24-V3`, 24 sorteadas, máximo 20; y `RIASEC-12-V2`, máximo 10) se siguen mostrando y recomendando sobre su propio máximo.

Los resultados del test anterior de 12 preguntas (`RIASEC-12-V2`, máximo 10 por dimensión) se siguen mostrando y generan recomendaciones: cada perfil se mide contra el máximo de su propio test, así una persona que obtuvo 10 de 10 tiene la misma compatibilidad que una que obtiene 20 de 20.

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
