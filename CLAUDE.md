# Orientación Vocacional (tesis)

<!-- ORION:inicio (sección generada por ORION; se actualiza al volver a preparar la carpeta) -->
## Tablero ORION (proyecto ORIVOC · Orientación Vocacional (tesis))

Este proyecto se gestiona en el tablero ORION. Trabajas sus tarjetas con el comando `orion` (ver `orion ayuda`).
Flujo: Backlog → Por hacer → En progreso → En revisión QA → **En revisión humano** (ahí terminas tú).
Una persona prueba la aplicación y mueve a Hecho o reporta nuevos errores. **Tú nunca mueves a Hecho.**

Cuando te pidan "trabaja el backlog de ORION" (o "toma la siguiente tarjeta"), repite para cada tarjeta:

1. `orion siguiente`: toma la tarjeta (pasa a Por hacer → En progreso y queda asignada a ti).
   Lee todo: descripción, criterios, comentarios, y abre las imágenes (`orion ver <N>` muestra sus rutas).
   - Si falta información para hacerla bien, no adivines: `orion bloquear <N> -c "qué falta"` y sigue con la siguiente.
2. Implementa **solo** lo que pide la tarjeta, en la rama actual (no crees ramas ni hagas push).
3. Haz **un commit por tarjeta** con el mensaje `<tipo>: <resumen> (ORION #<N>)` (tipo: fix, feat, refactor…)
   y regístralo: `orion trabajo <N> --commit <sha corto> --rama <rama actual>`.
4. `orion qa <N> -c "qué cambiaste y archivos clave"`: pasa a En revisión QA.
5. Verificación (QA): ejecuta las pruebas del proyecto (si las hay) y revisa el cambio. Revisa cada criterio de aceptación y márcalo con `orion criterio <N> <i>`.
   - Si algo falla, corrige, actualiza el mismo commit (`git commit --amend`), vuelve a registrar el sha y verifica de nuevo.
6. `orion entregar <N> -c "qué se verificó y cómo probarlo en la app, paso a paso"`: pasa a En revisión humano.
7. Continúa con la siguiente hasta que `orion siguiente` diga que no hay más o te pidan parar.
   Al final, resume qué tarjetas entregaste, cuáles bloqueaste y por qué.

Reglas: no toques tarjetas en "En revisión humano" ni en "Hecho"; no hagas `git push` salvo que te lo pidan;
responde y comenta en español.
<!-- ORION:fin -->
