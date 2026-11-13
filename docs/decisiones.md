# Decisiones

## 2026-11-03: Publicación en internet (`publicar-internet.local.ps1`)

Este script no se sube a git porque contiene la contraseña local de la base de datos, así que su cambio se documenta aquí.

**Síntoma:** "No se pudo obtener el enlace", con `tools\tunel.log` cortado en "Requesting new quick Tunnel".

**Qué se comprobó:**

- El Wi-Fi es la ruta de salida. Radmin VPN también está activo, pero con métrica mucho peor, así que no se lleva el tráfico.
- El puerto 7844 de Cloudflare responde por TCP y `api.trycloudflare.com` responde.
- Una primera llamada a Cloudflare falló y las siguientes funcionaron, lo que apunta a una conexión intermitente.
- La causa exacta de aquel fallo no quedó confirmada: `cloudflared` escribe sus errores fatales en la consola, no en `--logfile`, y su ventana se cerraba.

**Qué cambió:**

- El error real se guarda y se muestra en pantalla, con un consejo según el motivo.
- Hay 4 intentos: el primero deja elegir a `cloudflared` y los demás fuerzan `http2`, que usa TCP.
- El enlace solo se anuncia cuando hay una conexión registrada.
- La dirección `api.trycloudflare.com` ya no puede confundirse con el enlace.
- La API se detiene antes de compilarla. Antes el `.exe` bloqueado hacía fallar la compilación y se publicaba la versión anterior.
- Un fallo de compilación o una API que no responde detienen el script con un mensaje.
- Si el túnel falla, el script recuerda que la página sigue disponible en la PC y en la misma red Wi-Fi.

## 2026-11-04: Toda institución de Cochabamba que ofrezca la carrera

**Decisión:** el estudiante ve una tarjeta por carrera con las universidades e institutos de Cochabamba que la ofrecen, en vez de una tarjeta por institución.

**Alcance real:** "todas" significa todas las que tienen información comprobable. La prensa y un estudio municipal hablan de entre 72 y 170 institutos en Cochabamba, muchos sin sitio web, así que el catálogo no es exhaustivo y debe crecer desde el panel.

**Se agregaron:** UCB, UPAL, UNITEPC, Salesiana, EMI, ULAT, Unifranz, UPDS e INFOCAL. INCOS N.° 3 de Quillacollo se reactivó sin enlace porque no tiene sitio oficial.

**No se cargaron:** UDABOL y Nur (sus sitios no mencionan Cochabamba), Universidad Adventista (su sitio no permite leer la oferta sin navegador) e Instituto Álvarez Plata (sin evidencia de que exista en Cochabamba).

**Nombres:** las denominaciones oficiales distintas se unifican para agrupar (por ejemplo, Ciencias Jurídicas pasa a Derecho) y la fuente de cada carrera guarda el nombre original. Un título técnico y una licenciatura de nombre parecido siguen separados cuando su nombre difiere.

**Ventana interna:** si el servidor no logra comprobar un sitio en 10 segundos, ya no se asume que admite mostrarse dentro de la página: se ofrece otra pestaña y se reintenta al minuto. UNITEPC, Salesiana, EMI y UPEA prohíben la ventana interna.

**Pendiente:** en UPB, TECBA, ULAT, Unifranz y UPDS la página no indica la sede de cada carrera, por lo que quedan en `CONFIRMAR SEDE`, aunque el estudiante las ve. Los niveles de compatibilidad son una propuesta que debe aprobar el autor de la tesis.

## 2026-11-03: Catálogo completado desde los sitios oficiales

**Motivo:** las 4 universidades tenían 8 carreras estimadas por IA y ninguna regla para las dimensiones A y E, por lo que esos perfiles no tenían carreras afines.

**Fuentes:** [UMSS](https://www.umss.edu.bo/carreras/), [Univalle, sede Cochabamba](https://www.univalle.edu/?page_id=20106), [UPB](https://www.upb.edu/programas/pregrado) y las [facultades de USIP](https://usip.edu.bo/). Consultadas el 2026-11-03. Cada carrera guarda su fuente.

**Resultado:** 28 carreras visibles, con reglas en las 6 dimensiones.

| Estado | Significado |
|---|---|
| `VERIFICADA` | Figura en el sitio oficial, y en Univalle, en su sede Cochabamba |
| `CONFIRMAR SEDE` | Solo UPB: su página lista la oferta general y no indica la sede de cada carrera |
| `PENDIENTE` | Sin confirmar: las 5 instituciones ocultas y "Administración y Finanzas" de USIP |

**Correcciones a los datos de la IA:** "Derecho" de la UMSS figura como Ciencias Jurídicas e "Ingeniería de Sistemas" de la UPB como Ingeniería de Sistemas Computacionales. "Administración y Finanzas" no aparece entre las carreras de USIP, así que se ocultó sin borrarla. Los costos anteriores siguen guardados sin confirmar, y las carreras nuevas no tienen costo.

**Pendiente de aprobar:** los niveles de compatibilidad de las reglas nuevas son una propuesta y no vienen de las universidades: 85-90 para la dimensión principal y 45-70 para la secundaria, igual que las reglas existentes. Son un criterio de la tesis y su autor debe revisarlos.

**Cambio de datos:** el catálogo vive en la base y no en una migración, como antes. Está respaldado el estado anterior en `tools/respaldos/`.

## 2026-11-03: Actualización en vivo con consultas cada 5 segundos

**Decisión:** las pantallas preguntan cada 5 segundos si algo cambió, en vez de recibir un aviso del servidor.

**Motivo:** la técnica más simple para recibir avisos (SSE) no funciona en los túneles rápidos de Cloudflare, que son los que usa el enlace público. SignalR sí funcionaría, pero agrega una librería y una conexión abierta por estudiante, sin una ventaja que se note en esta aplicación.

**Consecuencias:** un cambio tarda hasta 5 segundos en llegar. Las pestañas en segundo plano no consultan. Refrescar no llama a la IA ni escribe en el historial, así que la explicación de la IA no cambia hasta volver a abrir la pantalla.

**Limitación:** el contador está en memoria y solo ve lo que pasa por la API. Un cambio hecho directamente en la base de datos no se detecta hasta recargar.

## 2026-11-03: Solo universidades de Cochabamba

**Decisión:** a pedido del usuario, el estudiante solo ve opciones de Cochabamba: UMSS, Univalle, UPB, USIP y TECBA. TECBA se ocultó primero por ser instituto y se reactivó el 2026-11-04 porque el usuario indicó, y su sitio confirma, que tiene sede en Cochabamba. Sus 2 carreras quedan en `CONFIRMAR SEDE` porque también tiene sede en Sucre.

UPEA e ITPM están en El Alto. INCOS y Álvarez Plata no tienen sitio oficial confirmado. Los cuatro quedan inactivos, no borrados: el administrador puede reactivarlos en Instituciones. Es un cambio de datos, por lo que no hay código que excluya a nadie.

## 2026-11-03: Enlaces oficiales en lugar de municipio, costo y lugar

**Motivo:** las 9 instituciones y las 18 carreras del catálogo tienen como fuente "Estimado con ayuda de IA (Gemini), sin confirmar". Ninguna carrera tiene duración, y al menos un municipio resultó falso: el Instituto Puerto de Mejillones figura en Quillacollo, pero su propio sitio y un plan de mejora publicado por CEMSE lo ubican en El Alto.

**Decisión:**

- El estudiante ya no ve costo, municipio ni modalidad, ni la IA los recibe.
- Al terminar el test ve las instituciones compatibles con su enlace oficial, que abre en una ventana dentro de la página.
- Los datos no se borraron: siguen en la base y el administrador los puede corregir.
- Los filtros siguen en el backend, sin uso desde la interfaz.

**Enlaces:** cada dirección se comprobó abriendo el sitio el 2026-11-03. INCOS e Instituto Álvarez Plata quedan sin enlace porque no se encontró una página oficial confirmable. El administrador puede agregarlo en Instituciones.

**Ventana interna:** el servidor comprueba las cabeceras `X-Frame-Options` y `frame-ancestors` de cada sitio, y guarda el resultado 6 horas. Si el sitio prohíbe mostrarse dentro de otra página (hoy UPEA), se ofrece abrirlo en otra pestaña. No se usa un proxy porque saltarse esa prohibición rompería los sitios y su seguridad.

## 2026-11-13: banco de 60 preguntas corregido, iguales para todos y sin cronómetro (ORION #27)

**Decisión:** el test usa el banco propio de 60 preguntas reescrito según la revisión de Gabriela Lino (Word `50 preguntas pagina web - REV1LUM.docx`). Todas empiezan con «¿Qué tanto te interesa…?» y su ejemplo se muestra debajo, en letra secundaria. Todos los estudiantes responden las mismas 60 (10 por dimensión), intercaladas, sin sorteo y sin cronómetro. La pantalla ya no muestra la dimensión de cada pregunta, y la instrucción pregunta por intereses: «Lee cada actividad y elige qué tanto te gusta o te interesa, aunque nunca la hayas realizado o todavía no sepas hacerla…». Versión `RIASEC-60-V4`, máximo 50 por dimensión.

**Motivo:** la revisión observó tres problemas: la instrucción hablaba de «forma de ser» (personalidad) y no de intereses; el cronómetro marcaba «Neutral» en lugar del estudiante; y mostrar la dimensión podía sesgar las respuestas. La forma fija responde a la crítica anterior de que, con el sorteo, cada estudiante respondía un test distinto.

**Se deshizo** el cambio al Perfil de Intereses O*NET (ORION #25, commit `2f79c01`). No se había publicado ni aplicado a ninguna base, y el usuario eligió el banco corregido. Queda en el historial de git.

**Correcciones de redacción** sobre el texto de la revisión: puntos finales, «ajedréz» → «un problema de ajedrez», «o un torno, un tractor» → «un torno o un tractor» y comas sueltas. R01 y R10 no tienen ejemplo porque la revisión no lo propuso.

**Limitación:** el banco propio no tiene validación psicométrica (el O*NET sí). Los puntajes de 60 preguntas (máximo 50) no se comparan directamente con los de 24 (máximo 20), salvo como porcentaje.

## 2026-11-13: el test vuelve a 24 preguntas, iguales para todos (ORION #28)

**Decisión:** cada intento presenta las mismas 24 preguntas a todos los estudiantes: las 4 primeras activas de cada dimensión del banco corregido (R01–R04, I01–I04, …), intercaladas, sin sorteo y sin cronómetro. Versión `RIASEC-24-V5`, máximo 20 por dimensión. Las otras 36 preguntas corregidas siguen en el banco; si el administrador desactiva una de las primeras, entra la siguiente activa de esa dimensión.

**Motivo:** el usuario prefirió un test más corto que el de 60 preguntas (ORION #27). Se conserva la forma fija para que los puntajes de distintos estudiantes se puedan comparar.

**Limitación:** 4 preguntas por dimensión dan menos confiabilidad que 10. Los resultados del test de 60 (`RIASEC-60-V4`, máximo 50) se siguen mostrando sobre su propio máximo.
