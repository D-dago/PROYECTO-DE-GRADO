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

## 2026-11-14: banco corregido por Ch'iti Wasi, casi sin ejemplos y sorteado en cada intento (ORION #30)

**Decisión:** las 60 preguntas toman el texto de la columna amarilla del Word `PREGUNTAS RIASEC - CORRECCIÓN CH'ITI WASI .docx` (revisión de Gabriela Lino), con solo correcciones de ortografía y puntuación («manual/practica» → «manual o práctica», «relacionados a» → «relacionados con», «Ejem;» → «Ejemplo:», tildes). Se quitan los ejemplos para que no guíen las respuestas; solo R02, R03, I09 y A02 conservan el ejemplo que la revisión reescribió. Cada intento vuelve a sortear 4 preguntas de cada área entre las activas (24 en total), intercaladas. Versión `RIASEC-24-V6`, máximo 20 por dimensión. Migración `BancoCorregidoChitiWasi` (solo actualiza los textos).

**Motivo:** la psicopedagoga aclaró las preguntas, diferenció las que parecían iguales y quitó los ejemplos para que los resultados sean más fiables. El usuario pidió que las preguntas sean aleatorias para que el estudiante no reciba siempre las mismas ni repita el test con las mismas respuestas hasta forzar un resultado. Esto reemplaza la forma fija de ORION #28.

**Detalles:** en el Word, la fila I05 repite el texto de I04 («hacer pruebas para comprobar si una idea es correcta»); se conserva la I05 anterior sin ejemplo («comparar datos para descubrir patrones») y hay que confirmarlo con la psicopedagoga. Con 4 de 10 por área hay 210⁶ combinaciones posibles: dos intentos prácticamente nunca traen las mismas 24, pero sí pueden compartir algunas. Las preguntas sorteadas quedan guardadas en el intento, así que uno a medias se reanuda con las mismas. Un intento a medias de `RIASEC-24-V5` no se reanuda: empieza uno nuevo. Los resultados de 24 fijas (`RIASEC-24-V5`), 60, 24 sorteadas del banco anterior y 12 preguntas se siguen mostrando sobre su propio máximo.

**Limitación:** con el sorteo, dos estudiantes no responden exactamente el mismo test, así que sus puntajes por dimensión no son estrictamente comparables (la crítica que motivó ORION #27). La migración debe aplicarse a la base de Neon antes de publicar; si no, el test sortearía los textos anteriores.

## 2026-11-14: cronómetro total que solo se ve al final del test (ORION #31)

**Decisión:** el resultado muestra «Tiempo para responder el test» (por ejemplo, «7 min 32 s»). Lo calcula el servidor con el inicio y el fin del intento (`StartedAtUtc` y `CompletedAtUtc`, que ya existían; no hace falta migración) y viaja en `durationSeconds` del resultado. Mientras se responde no se muestra ningún reloj.

**Motivo:** sugerencia en amarillo de la revisión de Ch'iti Wasi: en lugar del temporizador por pregunta (quitado en ORION #27), un cronómetro que mida cuánto tardó el cuestionario y que solo se vea al final.

**Limitación:** el tiempo cuenta desde que se abre el intento. Si el estudiante deja el test a medias y lo retoma otro día, incluye esa pausa. Los resultados sin fin registrado se muestran sin tiempo. El tiempo aparece en la pantalla de resultado al terminar; el historial no lo muestra.

## 2026-11-14: cada área afín muestra sus carreras y el catálogo se completó (ORION #32)

**Problema:** el resultado sugería «Agronomía y veterinaria», pero el buscador respondía «Ninguna carrera compatible coincide». El catálogo no tenía carreras de esa área, la búsqueda exigía la palabra exacta («agronomía» no encontraba «Agronómica») y varias áreas genéricas no se parecían al nombre de ninguna carrera.

**Decisión:**
- Cada área afín tiene términos de búsqueda definidos junto al área en `RiasecInterpretationBuilder` y enviados en `areaSearchTerms`. Al tocar o escribir el nombre del área se buscan esos términos en el nombre de las carreras. Un término con «-» excluye («fisica» sin «deporte»).
- La búsqueda escrita compara por raíz: sin las 2 últimas letras en palabras de 7 o más («agronomia» → «agronom»).
- Un área sin carreras en el catálogo se muestra atenuada y no como botón.
- Se revisó la oferta oficial de cada institución activa: 59 carreras agregadas y 10 ocultadas porque no se dictan en Cochabamba. El detalle, con las fuentes, está en [REVISION_CATALOGO_2026-11-14.md](REVISION_CATALOGO_2026-11-14.md). Hoy las 24 áreas posibles encuentran al menos una carrera.

**Limitación:** las reglas de compatibilidad de las carreras nuevas son una propuesta del sistema y el autor de la tesis debe revisarlas. Una carrera sin sede confirmada en Cochabamba no se agrega.

## 2026-11-14: solo se muestra lo confirmado en Cochabamba y vigente (ORION #33)

**Decisión:** se revisó todo lo oculto del catálogo para reactivar lo que fuera de Cochabamba y estuviera vigente. Nada cumplió las dos condiciones con una fuente oficial actual. Con el mismo criterio se ocultaron 8 carreras visibles: 5 de ULAT que ya no figuran en su sitio, 2 del INCOS estimadas con IA y Gastronomía de la UMSS, que se dicta en Cliza. El área afín «Gestión documental y secretariado» se reemplazó por «Comercio internacional y aduanas», porque ya no tenía carreras vigentes. Detalle y fuentes en [REVISION_CATALOGO_2026-11-14.md](REVISION_CATALOGO_2026-11-14.md).

**Criterio:** una carrera se muestra si una fuente oficial actual la ofrece y la institución está en Cochabamba. Las de UPB y TECBA siguen visibles con estado «confirmar sede»: la institución tiene sede en Cochabamba y la carrera figura en su oferta actual, aunque su web no indica la sede de cada carrera.
