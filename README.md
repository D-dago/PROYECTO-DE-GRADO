# Sistema web hibrido de orientacion vocacional

Base inicial para un sistema de orientacion vocacional dirigido a estudiantes bachilleres de Cochabamba, Bolivia.

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
- `POST /api/auth/logout`: cierre stateless; el cliente debe descartar el token.
- `GET /api/student/profile`: requiere rol `ESTUDIANTE`.
- `PUT /api/student/profile`: actualiza telefono, ciudad, colegio y ano de graduacion.
- `GET /api/admin/access`: requiere rol `ADMINISTRADOR` y deja preparada la superficie administrativa.

La migracion inicial crea `roles`, `users` y `student_profiles`, y siembra solo los roles `ESTUDIANTE` y `ADMINISTRADOR`. No crea usuarios, universidades, carreras ni datos institucionales.

## Test RIASEC

La migracion `RiasecTest` agrega exactamente 60 preguntas activas, diez por dimension (`R`, `I`, `A`, `S`, `E`, `C`), junto con `test_attempts`, `answers` y `riasec_profiles`.

Todos los endpoints requieren un JWT de estudiante:

- `GET /api/test/questions`
- `POST /api/test/attempts`
- `GET /api/test/attempts/{attemptId}`
- `POST /api/test/attempts/{attemptId}/answers` con `{ "questionId": "...", "value": 1 }`
- `POST /api/test/attempts/{attemptId}/complete`
- `GET /api/test/attempts/{attemptId}/result`

El frontend del quiz esta disponible en `/test`. Cada respuesta se guarda de inmediato y un intento en progreso se reutiliza al volver a entrar. El algoritmo `RIASEC-60-V1` suma cada dimension y resuelve empates con el orden fijo `R, I, A, S, E, C`.

Las pruebas se ejecutan con:

```powershell
cd backend
dotnet test OrientacionVocacional.Tests\OrientacionVocacional.Tests.csproj
```

## Comprobaciones

```powershell
cd frontend
npm run build

cd ..\backend
dotnet build
```

El test RIASEC, el motor de reglas, recomendaciones e IA opcional están implementados en esta etapa. La información educativa real queda pendiente de carga administrativa.

## Catálogo y recomendaciones

El catálogo no contiene universidades, carreras ni datos institucionales inventados. Se añadieron las tablas `institutions`, `careers`, `career_compatibilities` y `recommendation_history`; empiezan vacías y el administrador debe cargar información verificable con fuente y fecha.

El motor de reglas utiliza el perfil RIASEC guardado, compatibilidades por dimensión y filtros de institución, municipio, modalidad, nivel, área, costo máximo y duración máxima. La IA está detrás de `IAiExplanationService`; si no hay proveedor configurado, devuelve un mensaje de indisponibilidad y las recomendaciones estructuradas continúan funcionando.

Rutas frontend disponibles: `/registro`, `/login`, `/perfil`, `/test`, `/recomendaciones`, `/historial` y `/admin`.

Las operaciones `/api/admin/*` exigen el rol `ADMINISTRADOR`; las operaciones de estudiante usan el `UserId` del JWT y no aceptan identificadores de usuario enviados por el cliente.