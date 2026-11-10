# Guia de demostracion

## Preparacion

1. Configura `ConnectionStrings__PostgreSQL` y `Jwt__Key` en PowerShell.
2. Desde `backend`, ejecuta `dotnet ef database update --project OrientacionVocacional.Infrastructure --startup-project OrientacionVocacional.API`.
3. Inicia la API con `dotnet run --project OrientacionVocacional.API`.
4. En otra terminal, inicia `npm run dev` desde `frontend`.
5. Abre `http://localhost:5173`.

Para comprobar antes de empezar que todo está encendido: `GET /health` debe responder `Healthy`.

## Flujo estudiante

1. Abre `/registro` y crea una cuenta (o entra con Google).
2. Entra a `/login`.
3. Abre `/perfil` y guarda datos académicos.
4. Abre `/test` y responde las 24 preguntas (4 por cada una de las 6 dimensiones RIASEC, sorteadas del banco de 60).
5. Muestra el resultado: código del perfil, tarjetas por dimensión, puntajes, explicación y el aviso de orientación no diagnóstica.
6. Abre `/recomendaciones`; inicialmente puede mostrar que no existen alternativas si el catálogo está vacío.
7. Muestra las carreras compatibles, con las universidades e institutos de Cochabamba que las ofrecen, y abre un sitio oficial en la ventana interna.
8. Abre `/historial`: hay una entrada por test terminado y se despliega con el detalle completo del perfil.
9. Abre `/skins` para mostrar las monedas ganadas y el cambio de avatar.
10. Cierra sesión con el botón "Salir".

## Flujo administrador

1. Usa una cuenta con rol `ADMINISTRADOR` provisionada en PostgreSQL.
2. Abre `/admin` y muestra el dashboard.
3. Carga una institución con nombre, tipo, fuente y estado verificados.
4. Carga una carrera asociada a la institución.
5. Crea una regla de compatibilidad desde el formulario de la sección "Compatibilidades por dimensión (RIASEC)".
6. Repite `/recomendaciones` como estudiante para mostrar la regla aplicada.
7. En "Resultados", despliega un estudiante para ver la interpretación de su perfil y descarga el Excel y el PDF.
8. En "Preguntas", edita o desactiva una pregunta y muestra que el sistema impide dejar una dimensión con menos de 2 activas.
9. Prueba activación/desactivación de instituciones, carreras y estudiantes.

No uses registros inventados como información real. Si la base de datos está vacía, demuestra el estado vacío y explica que el administrador debe cargar fuentes verificables.

## Evidencias técnicas

- `dotnet build`
- `dotnet test`
- `npm run build`
- Solicitud sin JWT a `/api/test/questions`: `401`.
- Solicitud sin JWT a `/api/admin/catalog/dashboard`: `401`.
- Estudiante contra `/api/admin/*`: debe recibir `403`.
- `GET /health`: `Healthy`.
- Cada push ejecuta el flujo de CI de GitHub Actions (`.github/workflows/ci.yml`), que compila el backend, corre las pruebas y compila el frontend.
