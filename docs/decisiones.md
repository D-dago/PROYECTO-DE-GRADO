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

## 2026-11-03: Enlaces oficiales en lugar de municipio, costo y lugar

**Motivo:** las 9 instituciones y las 18 carreras del catálogo tienen como fuente "Estimado con ayuda de IA (Gemini), sin confirmar". Ninguna carrera tiene duración, y al menos un municipio resultó falso: el Instituto Puerto de Mejillones figura en Quillacollo, pero su propio sitio y un plan de mejora publicado por CEMSE lo ubican en El Alto.

**Decisión:**

- El estudiante ya no ve costo, municipio ni modalidad, ni la IA los recibe.
- Al terminar el test ve las instituciones compatibles con su enlace oficial, que abre en una ventana dentro de la página.
- Los datos no se borraron: siguen en la base y el administrador los puede corregir.
- Los filtros siguen en el backend, sin uso desde la interfaz.

**Enlaces:** cada dirección se comprobó abriendo el sitio el 2026-11-03. INCOS e Instituto Álvarez Plata quedan sin enlace porque no se encontró una página oficial confirmable. El administrador puede agregarlo en Instituciones.

**Ventana interna:** el servidor comprueba las cabeceras `X-Frame-Options` y `frame-ancestors` de cada sitio, y guarda el resultado 6 horas. Si el sitio prohíbe mostrarse dentro de otra página (hoy UPEA), se ofrece abrirlo en otra pestaña. No se usa un proxy porque saltarse esa prohibición rompería los sitios y su seguridad.
