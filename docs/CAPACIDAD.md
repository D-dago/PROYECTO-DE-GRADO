# Capacidad: ¿cuántos estudiantes a la vez?

Medido el 2026-11-11 con pruebas de carga que simulan a estudiantes de **un mismo colegio**. Todos salen a internet por la **misma IP**, que es el caso real de un aula o un laboratorio. Cada estudiante simulado:

1. se registra;
2. abre el test y responde las 24 preguntas;
3. ve su resultado, "Dónde estudiar" y su historial.

Mientras tanto, su pantalla consulta cada 5 s si hubo cambios.

## Resumen

| Situación | Antes (ORION #19) | Ahora (ORION #20) |
| --- | --- | --- |
| Límite anti-abuso | Por IP: 40 registros/hora, 30 inicios de sesión/min y 300 pedidos/min para **todo el colegio** | Por estudiante con sesión (300 pedidos/min cada uno); por IP solo sin sesión (600 registros/hora, 600 inicios/min) |
| 40 estudiantes a la vez (PC normal) | **Ninguno terminó**: 819 de 960 respuestas rechazadas (429) | Todos terminan |
| 120 estudiantes a la vez (PC normal) | 80 de 120 no pudieron ni registrarse | 120 de 120 terminan, 0 errores; respuesta mediana 13 ms |
| 200 estudiantes a la vez (PC normal) | — | 200 de 200 terminan, 0 errores; respuesta mediana 11 ms, p95 18 ms |
| Adivinar contraseñas | Frenado por IP (bloqueaba al colegio entero) | Frenado por cuenta: 10 fallos en 15 min bloquean solo ese correo |

## Qué limita en Render gratis

Render gratis da **0,1 de CPU y 512 MB**. Para medirlo, la API se ejecutó con un tope duro de 0,1 de un núcleo (Job Object de Windows) y con .NET viendo un solo procesador, igual que en Render.

- **Un estudiante cuesta unos 0,23 s de CPU** en todo su recorrido (medido con 200 estudiantes). Unos 0,075 s son su registro.
- **Lo más costoso es la contraseña**: cada registro o inicio de sesión calcula un hash PBKDF2 (100 000 iteraciones). Con 0,1 de CPU cada uno tarda alrededor de 1 s. Si 100 estudiantes pulsan "Entrar" en el mismo momento, el último espera cerca de 2 minutos.
- **Antes**, esa espera terminaba en error: internet corta los pedidos que tardan más de 100 s, y 55 de 100 registros se perdieron.
- **Ahora** hay una **fila ordenada**:
  - Las contraseñas se procesan por turnos y no frenan a quienes ya están respondiendo.
  - Si la espera superaría 40 s, el servidor contesta al instante con cuántos segundos esperar.
  - La página reintenta sola, con el aviso "Estás en fila; seguimos intentando solos".
  - El botón queda desactivado para que nadie lo pulse varias veces.
- **Responder el test es barato**: menos de 5 ms de CPU por respuesta.

### Medición con CPU limitada: 100 estudiantes que empiezan juntos

Los 100 estudiantes llegan en los primeros 10 s y responden una pregunta cada 4 a 8 s (ritmo de una persona leyendo).

| CPU del servidor | Terminaron el test | Registro (con fila) | Responder (mediana / p95) |
| --- | --- | --- | --- |
| 0,1 (Render gratis), versión anterior | 43 de 100 (55 registros cortados a los 2 min) | mediana 120 s | 1,1 s / 9,3 s |
| 0,1 (Render gratis), versión nueva | 34 de 100 | 57 entraron, 43 se cortaron | 1,7 s / 10 s |
| **0,5 (Render Starter), versión nueva** | **100 de 100, 0 errores** | **mediana 15 s, máximo 30 s** | **17 ms / 327 ms** |

La simulación de 0,1 de CPU en Windows probablemente es algo más dura que Render real, que reparte la CPU de forma más pareja. Aun así, la conclusión es clara: **con 0,1 de CPU no alcanza para que 100 estudiantes se registren al mismo tiempo**. Con 0,5 de CPU sobra.

## Límites de Neon gratis (base de datos)

- **100 horas de cómputo al mes.** La base se suspende sola tras 5 minutos sin uso, y el aviso de UptimeRobot a `/health/live` no la despierta porque no consulta la base. Una jornada de clases usa unas pocas horas.
- **1 GB de almacenamiento.** Un test terminado ocupa unos pocos KB, así que caben decenas de miles.
- **Hasta unas 100 conexiones** en el tamaño mínimo. La API usa como máximo 20 (`Maximum Pool Size=20`).
- Si se agotan las horas de cómputo, Neon suspende la base hasta el mes siguiente. Se puede revisar el consumo en el panel de Neon, en *Usage*.

## Recomendaciones

1. **Para 100 o más estudiantes a la vez: Render Starter** (unos 7 USD al mes, 0,5 de CPU). Medido: 100 de 100 terminan, el registro tarda como mucho 30 s y cada respuesta menos de medio segundo. Se puede subir solo el mes de las pruebas con estudiantes y volver al plan gratis después (en Render: servicio *orientacion-vocacional* → *Settings* → *Instance Type*).
2. **En Render gratis**: funciona bien con **grupos de unos 20 a 30 estudiantes a la vez**. Para un colegio entero, que entren por cursos (un curso cada 5 minutos) o que creen su cuenta el día anterior, porque registrarse e iniciar sesión es lo pesado.
3. No hace falta cambiar Neon mientras no se agoten las 100 horas al mes.

## Cómo repetir la medición

Los scripts usados están descritos en la tarjeta ORION #20:

- `carga.mjs` simula N estudiantes con una pausa de X a Y ms entre respuestas.
- `costo.mjs` y `costo2.mjs` miden la CPU por tipo de pedido.
- `servidor-limitado.ps1` arranca la API con el tope de CPU.

Se ejecutaron contra una copia de la base local (`orientacion_e2e`), nunca contra la base de internet.

## Nota: test de 60 preguntas (13-nov-2026, ORION #27; desde ORION #28 vuelven a ser 24)

Las mediciones de arriba se hicieron con el test de 24 preguntas. Con 60, cada estudiante envía 36 respuestas más. Cada respuesta cuesta menos de 5 ms de CPU, así que el recorrido completo sube de unos 0,23 s a unos 0,4 s de CPU por estudiante. Las respuestas se reparten en más tiempo (el test dura más). Lo pesado sigue siendo registrarse e iniciar sesión, que no cambia, así que las recomendaciones se mantienen.

Fuentes de los límites: [planes de Render](https://render.com/docs/compute-plans), [límites del plan gratis de Neon](https://neon.com/faqs/free-plan-limits-and-quotas).
