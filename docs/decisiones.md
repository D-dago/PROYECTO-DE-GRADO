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

**Decisión:** a pedido del usuario, el estudiante solo ve UMSS, Univalle, UPB y USIP.

UPEA e ITPM están en El Alto. TECBA, INCOS y Álvarez Plata son institutos, no universidades. Los cinco quedan inactivos, no borrados: el administrador puede reactivarlos en Instituciones. Es un cambio de datos, por lo que no hay código que excluya a nadie.

## 2026-11-03: Enlaces oficiales en lugar de municipio, costo y lugar

**Motivo:** las 9 instituciones y las 18 carreras del catálogo tienen como fuente "Estimado con ayuda de IA (Gemini), sin confirmar". Ninguna carrera tiene duración, y al menos un municipio resultó falso: el Instituto Puerto de Mejillones figura en Quillacollo, pero su propio sitio y un plan de mejora publicado por CEMSE lo ubican en El Alto.

**Decisión:**

- El estudiante ya no ve costo, municipio ni modalidad, ni la IA los recibe.
- Al terminar el test ve las instituciones compatibles con su enlace oficial, que abre en una ventana dentro de la página.
- Los datos no se borraron: siguen en la base y el administrador los puede corregir.
- Los filtros siguen en el backend, sin uso desde la interfaz.

**Enlaces:** cada dirección se comprobó abriendo el sitio el 2026-11-03. INCOS e Instituto Álvarez Plata quedan sin enlace porque no se encontró una página oficial confirmable. El administrador puede agregarlo en Instituciones.

**Ventana interna:** el servidor comprueba las cabeceras `X-Frame-Options` y `frame-ancestors` de cada sitio, y guarda el resultado 6 horas. Si el sitio prohíbe mostrarse dentro de otra página (hoy UPEA), se ofrece abrirlo en otra pestaña. No se usa un proxy porque saltarse esa prohibición rompería los sitios y su seguridad.
