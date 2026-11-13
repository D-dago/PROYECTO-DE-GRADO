# Sistema web hibrido de orientacion vocacional

Sistema de orientacion vocacional para estudiantes bachilleres de Cochabamba, Bolivia: test vocacional RIASEC (Holland), motor de reglas con el catalogo educativo cargado por el administrador, explicacion opcional con IA y panel administrativo.

## Estructura

- `frontend`: React, TypeScript y Vite.
- `backend`: ASP.NET Core Web API por capas.
- `database`: espacio para migraciones y scripts de PostgreSQL.
- `docs`: documentacion del proyecto.

El backend separa `API`, `Application`, `Domain` e `Infrastructure`. Entity Framework Core y Npgsql estan preparados en Infrastructure. No se incluyen credenciales reales.

## Requisitos

- Node.js y npm.
- .NET SDK 10.
- PostgreSQL 16 o compatible cuando se empiece a usar persistencia.

## Ejecutar frontend

```powershell
cd frontend
npm install
npm run dev
```

El frontend queda disponible normalmente en `http://localhost:5173`.

## Ejecutar backend

```powershell
cd backend
dotnet restore
dotnet run --project OrientacionVocacional.API
```

Con el comando documentado y `launchSettings.json`, la API queda normalmente en `http://localhost:5194`. Si se inicia con `--urls http://localhost:5000`, queda en `http://localhost:5000`. La cadena de conexion se puede establecer con `ConnectionStrings__PostgreSQL` en el entorno. `backend/.env.example` documenta el formato, pero ASP.NET Core no carga archivos `.env` automaticamente.

Antes de iniciar el backend, configura una cadena PostgreSQL real y una clave JWT de al menos 32 bytes. En PowerShell, por ejemplo:

```powershell
$env:ConnectionStrings__PostgreSQL = "Host=localhost;Port=5432;Database=orientacion_vocacional;Username=postgres;Password=TU_CLAVE"
$env:Jwt__Key = "TU_SECRETO_LOCAL_DE_AL_MENOS_32_BYTES"
```

## Migraciones y autenticacion

```powershell
cd backend
dotnet ef database update --project OrientacionVocacional.Infrastructure --startup-project OrientacionVocacional.API
dotnet run --project OrientacionVocacional.API
```

Endpoints principales:

- `POST /api/auth/register`: crea un estudiante y su perfil vacio.
- `POST /api/auth/login`: devuelve el JWT.
- `POST /api/auth/google`: inicio de sesion con Google; valida el idToken y emite el mismo JWT propio.
- `POST /api/auth/logout`: cierre stateless; el cliente debe descartar el token.
- `GET /api/student/profile`: requiere rol `ESTUDIANTE`.
- `PUT /api/student/profile`: actualiza telefono, ciudad, colegio y ano de graduacion.
- `GET /api/admin/access`: requiere rol `ADMINISTRADOR` y deja preparada la superficie administrativa.

La migracion inicial crea `roles`, `users` y `student_profiles`, y siembra solo los roles `ESTUDIANTE` y `ADMINISTRADOR`. No crea usuarios, universidades, carreras ni datos institucionales.

## Test vocacional RIASEC

El banco tiene 60 preguntas activas, diez por cada una de las seis dimensiones: `R` (Realista), `I` (Investigador), `A` (Artistico), `S` (Social), `E` (Emprendedor) y `C` (Convencional), segun la teoria de Holland. El sistema llego a aplicar por un tiempo una version CHASIDE del test; la migracion `VuelveATestRiasec` restauro el banco RIASEC y dejo inactivas las preguntas propias de CHASIDE (areas H y D), porque hay respuestas guardadas que las referencian.

Cada intento presenta las mismas 24 preguntas a todos (las 4 primeras de cada dimension del banco de 60, sin sorteo ni limite de tiempo) y las intercala para que no salgan dos seguidas de la misma dimension. El algoritmo `RIASEC-24-V5` suma cada dimension y resuelve empates con el orden fijo `R, I, A, S, E, C`; el puntaje maximo por dimension es 20 (4 preguntas x 5). Los resultados de versiones anteriores (60, 24 sorteadas y 12 preguntas) se siguen mostrando sobre su propio maximo. Los resultados del periodo CHASIDE se conservan, pero no se interpretan ni generan recomendaciones.

Todos los endpoints requieren un JWT de estudiante:

- `POST /api/test/attempts`: crea el intento con sus 24 preguntas (o devuelve el intento en progreso).
- `GET /api/test/attempts/{attemptId}`
- `GET /api/test/attempts/{attemptId}/questions`: las preguntas del intento, en orden.
- `POST /api/test/attempts/{attemptId}/answers` con `{ "questionId": "...", "value": 1 }`
- `POST /api/test/attempts/{attemptId}/complete`
- `GET /api/test/attempts/{attemptId}/result`

El quiz esta en `/test`. Cada respuesta se guarda de inmediato y un intento en progreso se reutiliza al volver a entrar. Responder da monedas (1 por respuesta, 50 al terminar) que se gastan en el catalogo de skins de `/skins`.

Las pruebas se ejecutan con:

```powershell
cd backend
dotnet test
```

## Comprobaciones

```powershell
cd frontend
npm run build

cd ../backend
dotnet build
dotnet test
```

Cada push y cada pull request ejecutan lo mismo en GitHub Actions (`.github/workflows/ci.yml`).

## Catálogo y recomendaciones

El catálogo no contiene universidades, carreras ni datos institucionales inventados. Se añadieron las tablas `institutions`, `careers`, `career_compatibilities` y `recommendation_history`; empiezan vacías y el administrador debe cargar información verificable con fuente y fecha.

El motor de reglas utiliza el perfil vocacional guardado, compatibilidades por dimensión RIASEC y filtros de institución, municipio, modalidad, nivel, área, costo máximo y duración máxima. Los filtros de texto ignoran mayúsculas y tildes. La IA está detrás de `IAiExplanationService`; si no hay proveedor configurado, devuelve un mensaje de indisponibilidad y las recomendaciones estructuradas continúan funcionando.

Rutas frontend disponibles: `/registro`, `/login`, `/perfil`, `/test`, `/recomendaciones`, `/historial`, `/skins` y `/admin`.

El historial guarda una entrada por test terminado (no una por búsqueda) con el perfil, los puntajes y la interpretación de esa vez.

## Panel de administrador

Desde `/admin`, una cuenta con rol `ADMINISTRADOR` gestiona instituciones, carreras, reglas de compatibilidad, el banco de preguntas y los estudiantes, y consulta los resultados con su interpretación, exportables a Excel (con el detalle de respuestas) y a PDF. Una pregunta no se puede desactivar si deja a su dimensión con menos de 2 activas.

## Operación

- `GET /health` comprueba que la API responde y alcanza la base de datos.
- En desarrollo, la documentación interactiva de la API queda en `/swagger`.
- Hay límites de tamaño (100 KB por petición) y de frecuencia por IP para registro, login y uso general.

Las operaciones `/api/admin/*` exigen el rol `ADMINISTRADOR`; las operaciones de estudiante usan el `UserId` del JWT y no aceptan identificadores de usuario enviados por el cliente.