# Guia de uso del sistema

## 1. REQUISITOS PREVIOS

Instala Node.js, npm, .NET SDK 10, PostgreSQL y Visual Studio Code.

## 2. CONFIGURACION DEL PROYECTO

El frontend esta en `frontend`, el backend en `backend` y las migraciones/documentacion de base de datos viven en `database` y `backend/OrientacionVocacional.Infrastructure/Persistence/Migrations`.

Configura variables de entorno en PowerShell o User Secrets. No guardes secretos en React ni en Git:

```powershell
$env:ConnectionStrings__PostgreSQL = "Host=localhost;Port=5432;Database=orientacion_vocacional;Username=postgres;Password=TU_CLAVE"
$env:Jwt__Key = "UN_SECRETO_LOCAL_DE_AL_MENOS_32_BYTES"
```

La IA es opcional. Actualmente el sistema usa un proveedor degradable y continua funcionando sin API key.

## 3. CREAR/ACTUALIZAR BASE DE DATOS

Desde `backend`, ejecuta:

```powershell
dotnet ef database update --project OrientacionVocacional.Infrastructure --startup-project OrientacionVocacional.API
```

## 4. INICIAR BACKEND

Desde `backend`:

```powershell
dotnet run --project OrientacionVocacional.API
```

Con la configuración actual de `launchSettings.json`, la API queda normalmente en `http://localhost:5194`.

## 5. INICIAR FRONTEND

Desde `frontend`:

```powershell
npm install
npm run dev
```

## 6. ABRIR EL SISTEMA

Abre `http://localhost:5173`.

## 7. CREAR UNA CUENTA DE ESTUDIANTE

Entra a `/registro`, escribe nombre, apellido, correo y contraseña. Luego entra a `/login`. Después abre `/perfil` para revisar y guardar teléfono, ciudad, colegio y año de graduación.

## 8. REALIZAR EL TEST VOCACIONAL (RIASEC)

Abre `/test` y responde las 12 preguntas tocando una opción de 1 a 5: Nada, Poco, Neutral, Mucho o Totalmente. Cada intento sortea 2 preguntas por cada una de las 6 dimensiones desde un banco de 60, así que dos intentos no traen las mismas preguntas. Cada respuesta se guarda inmediatamente. Si abandonas, el intento en progreso se reutiliza al volver.

Cada respuesta da 1 moneda y terminar el test da 50; las monedas se gastan en `/skins`.

## 9. INTERPRETAR EL RESULTADO

Las seis dimensiones RIASEC (teoría de Holland) son: `R` Realista, `I` Investigador, `A` Artístico, `S` Social, `E` Emprendedor y `C` Convencional. El código principal usa las dos dimensiones con mayor puntuación y el secundario la tercera; los empates se resuelven en el orden fijo `R, I, A, S, E, C`. El puntaje máximo por dimensión es 10.

La pantalla de resultado explica qué significa cada letra del perfil, muestra los puntajes, las áreas profesionales afines y las carreras del catálogo ordenadas por compatibilidad. Es una herramienta de orientación vocacional, no un diagnóstico psicológico ni determina obligatoriamente una profesión.

## 10. OBTENER RECOMENDACIONES

El flujo es: resultado del test, reglas deterministas, compatibilidad y universidades recomendadas. Aparecen al terminar el test y también en `/recomendaciones`.

## 11. USAR LOS FILTROS

Se muestran las universidades de Cochabamba. Cada una presenta sus carreras más compatibles y el botón **Ver sitio oficial**, que abre su página en una ventana dentro del sistema. Si la institución no permite mostrarse dentro de otra página, se ofrece abrirla en otra pestaña. Costos, ubicación y requisitos se consultan en ese sitio oficial. Solo se muestran registros activos; una institución sin página confirmada aparece sin enlace.

## 12. COMPARAR CARRERAS

Selecciona `Comparar` en dos o más alternativas. La vista compara compatibilidad, institución, modalidad y duración. Cuando falte un dato aparece `Información pendiente de actualización`.

## 13. USAR LA IA

La IA recibirá únicamente el perfil vocacional y alternativas estructuradas producidas por reglas. No calcula puntuaciones, no modifica reglas y no inventa carreras o instituciones. Si no existe configuración de proveedor, se muestra: `Explicación con IA no disponible actualmente. Las recomendaciones estructuradas continúan disponibles.`

## 14. HISTORIAL

Abre `/historial` para consultar tus resultados. Hay una entrada por test terminado —no una por búsqueda— y cada una se despliega con el perfil, los puntajes y la interpretación completa de esa vez, aunque más tarde repitas el test y te salga un perfil distinto. El backend filtra siempre por el usuario incluido en el JWT.

## 15. ADMINISTRADOR

El administrador debe iniciar sesión con una cuenta que tenga el rol `ADMINISTRADOR`. La ruta `/admin` permite:

- Cargar y editar instituciones y carreras con fuente y estado, y activarlas o desactivarlas.
- Crear, editar y eliminar reglas de compatibilidad por dimensión RIASEC.
- Ver los resultados de cada estudiante con su interpretación, y descargarlos en Excel (incluye el detalle de respuestas) o PDF.
- Editar el banco de preguntas. No se puede dejar una dimensión con menos de 2 preguntas activas, porque el test no se podría armar.
- Ver los estudiantes registrados con sus monedas y tests terminados, y activarlos o desactivarlos.

El backend protege todas estas operaciones con autorización por rol. No se crean datos institucionales ficticios.

## 16. INSTALACION

Clona o abre el workspace, ejecuta `npm install` en `frontend` y `dotnet restore` en `backend`.

## 17. POSTGRESQL

El servicio PostgreSQL debe estar iniciado. Coloca la contraseña real únicamente en `ConnectionStrings__PostgreSQL`; nunca en React, Git o archivos públicos.

## 18. VARIABLES DE ENTORNO

Configura `ConnectionStrings__PostgreSQL`, `Jwt__Key`, `Jwt__Issuer`, `Jwt__Audience` y opcionalmente `AI__ProviderUrl`, `AI__Model` y `AI__ApiKey`.

## 19. MIGRACIONES

Ejecuta `dotnet ef database update --project OrientacionVocacional.Infrastructure --startup-project OrientacionVocacional.API` desde `backend`.

## 20. INICIAR SERVICIOS

Inicia el backend con `dotnet run --project OrientacionVocacional.API` y el frontend con `npm run dev` en otra terminal.

## 21. RESULTADOS Y RECOMENDACIONES

El resultado conserva las seis puntuaciones, código principal, secundario y versión del algoritmo (`RIASEC-12-V2`). Un resultado de otra versión del test (por ejemplo, del período en que se aplicó CHASIDE) se conserva pero ya no se interpreta ni genera recomendaciones: sus letras significan otra cosa y habría que repetir el test. Las recomendaciones solo aparecen cuando existen carreras y reglas cargadas.

## 22. FUENTES Y FECHAS

Cada institución y carrera debe registrar fuente y fecha de actualización. Si el dato no existe, la interfaz muestra `Información pendiente de actualización`.

## 23. SEGURIDAD

Las operaciones administrativas requieren `ADMINISTRADOR`. Los endpoints de estudiante filtran por el usuario del JWT. Los estudiantes no reciben contraseñas ni datos de otros usuarios.

## 24. PRUEBAS

Ejecuta `dotnet build`, `dotnet test` y `npm run build`. Las pruebas cubren el banco de preguntas, la escala, los duplicados, la finalización, el determinismo del algoritmo, el aislamiento entre estudiantes, el panel de administrador y las exportaciones. Además, cada push ejecuta el flujo de CI en `.github/workflows/ci.yml`.

## 25. SOLUCION DE PROBLEMAS

Para comprobar de un vistazo que la API responde y alcanza la base de datos, abre `GET /health`: debe decir `Healthy`. En desarrollo también está Swagger en `/swagger`.

Si aparece un error de contraseña, configura la contraseña real de PostgreSQL. Si la IA no está configurada, el mensaje de indisponibilidad es esperado y las recomendaciones estructuradas continúan funcionando. Si el catálogo está vacío, carga datos verificables desde el panel administrativo.

## Pruebas

Desde `backend` ejecuta `dotnet build` y `dotnet test`. Desde `frontend` ejecuta `npm run build`.
