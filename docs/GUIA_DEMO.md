# Guia de demostracion

## Preparacion

1. Configura `ConnectionStrings__PostgreSQL` y `Jwt__Key` en PowerShell.
2. Desde `backend`, ejecuta `dotnet ef database update --project OrientacionVocacional.Infrastructure --startup-project OrientacionVocacional.API`.
3. Inicia la API con `dotnet run --project OrientacionVocacional.API`.
4. En otra terminal, inicia `npm run dev` desde `frontend`.
5. Abre `http://localhost:5173`.

## Flujo estudiante

1. Abre `/registro` y crea una cuenta.
2. Entra a `/login`.
3. Abre `/perfil` y guarda datos académicos.
4. Abre `/test` y responde las 60 preguntas.
5. Muestra el resultado RIASEC y el aviso de orientación no diagnóstica.
6. Abre `/recomendaciones`; inicialmente puede mostrar que no existen alternativas si el catálogo está vacío.
7. Explica los filtros y la comparación.
8. Abre `/historial` para mostrar el registro de la consulta.
9. Cierra sesión eliminando el token del navegador.

## Flujo administrador

1. Usa una cuenta con rol `ADMINISTRADOR` provisionada en PostgreSQL.
2. Abre `/admin` y muestra el dashboard.
3. Carga una institución con nombre, tipo, fuente y estado verificados.
4. Carga una carrera asociada a la institución.
5. Crea una compatibilidad mediante `POST /api/admin/catalog/compatibilities`.
6. Repite `/recomendaciones` como estudiante para mostrar la regla aplicada.
7. Prueba activación/desactivación y edición mediante los endpoints protegidos.

No uses registros inventados como información real. Si la base de datos está vacía, demuestra el estado vacío y explica que el administrador debe cargar fuentes verificables.

## Evidencias técnicas

- `dotnet build`
- `dotnet test`
- `npm run build`
- Solicitud sin JWT a `/api/test/questions`: `401`.
- Solicitud sin JWT a `/api/admin/catalog/dashboard`: `401`.
- Estudiante contra `/api/admin/*`: debe recibir `403`.
