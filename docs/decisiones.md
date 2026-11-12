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

## 2026-11-12: el test aplica el Perfil de Intereses O*NET en español (ORION #25)

**Decisión:** el test deja el banco propio de 60 preguntas (se sorteaban 4 por dimensión) y aplica las 30 actividades del Perfil de Intereses O*NET en español, sin cambios, en forma fija y en el orden oficial. Escala O*NET (Me disgusta mucho … Me gusta mucho), puntaje 0-4 por respuesta y 0-20 por dimensión, y perfil calculado como O*NET: solo cuentan las dimensiones sobre el promedio del propio estudiante, hasta tres, con desempate por el hexágono; sin ninguna, perfil plano sin código. Versión `ONET-30-V4`.

**Motivo:** una psicopedagoga observó el banco y el método. La revisión (`Desktop/CORRESIONES 2/REVISION_BANCO_PREGUNTAS_RIASEC.docx`) confirmó que, con el sorteo, cada estudiante respondía un test distinto y los puntajes no eran comparables; que 4 ítems por dimensión dan una confiabilidad baja; que la escala Nada … Totalmente no separaba "me disgusta" de "me da igual"; y que muchos ítems medían hábitos o rasgos, no intereses. El O*NET es gratuito, está validado y su licencia permite usarlo con atribución.

**Detalles:** códigos `OR1` a `OC5` (el administrador solo genera letra + número, así que no chocan). El test toma las 5 primeras preguntas activas de cada dimensión por su orden: si el administrador desactiva una actividad, entra la siguiente activa. La pantalla ya no muestra la dimensión de cada actividad (O*NET tampoco). Los resultados de 24 y 12 preguntas se siguen mostrando con su propia puntuación.

**Limitaciones:** cambiar un texto O*NET obliga, según la licencia, a avisar que USDOL/ETA no aprobó la modificación y a validar la versión nueva. El cronómetro de 30 s que marca "No estoy seguro/a" al vencerse no es parte del O*NET. La migración `TestPerfilInteresesOnet` debe aplicarse a la base de Neon antes de publicar este cambio; si no, el test aplicaría el banco anterior con la puntuación nueva.
