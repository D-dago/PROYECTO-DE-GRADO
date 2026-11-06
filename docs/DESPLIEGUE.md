# Publicar en Render con la base en Neon (gratis y sin tarjeta)

La página corre en **Render** (plan gratuito) y los datos se guardan en una base PostgreSQL de **Neon** (plan gratuito). Render duerme la página tras 15 minutos sin visitas, y un aviso de **UptimeRobot** cada 5 minutos la mantiene despierta. Las 750 horas gratis al mes de Render alcanzan para tener un servicio encendido todo el mes.

## Qué se publica

- El `Dockerfile` compila la web y la API en una sola imagen. `render.yaml` le indica a Render cómo publicarla.
- La base de la nube **no es la de la PC**. Lleva el catálogo (instituciones, carreras, reglas y preguntas), la cuenta de administrador y la de estudiante de prueba. Los estudiantes reales quedan solo en la PC. El archivo que se sube es `tools/respaldos/para-la-nube-2026-11-05.dump`, que no se sube a git.

## Pasos

1. **Neon**
   1. Crea una cuenta en <https://neon.tech>; puedes entrar con GitHub.
   2. Crea un proyecto en la región **AWS US East (N. Virginia)**, que es la de Render.
   3. Copia la *Connection string* y guárdala en `neon.local.txt`, en la raíz del proyecto. Ese archivo no se sube a git.
   4. Con eso se carga la base:

      ```bash
      pg_restore --no-owner --no-privileges -d "<cadena de Neon>" tools/respaldos/para-la-nube-2026-11-05.dump
      ```

2. **Render**
   1. Crea una cuenta en <https://render.com> entrando con GitHub.
   2. Elige *New > Blueprint* y selecciona el repositorio `PROYECTO-DE-GRADO`.
   3. Render pide `ConnectionStrings__PostgreSQL`. Pega la cadena en formato Npgsql (ver la tabla de abajo).
   4. Pulsa *Apply*. La primera publicación tarda unos 10 minutos. La dirección queda como `https://orientacion-vocacional.onrender.com` o con un sufijo si ese nombre está ocupado.

3. **UptimeRobot**
   1. Crea una cuenta en <https://uptimerobot.com>.
   2. Crea un monitor *HTTP(s)* hacia `https://<tu-direccion>.onrender.com/health/live`, cada **5 minutos**.
   3. Esa ruta no consulta la base, así que Neon puede suspenderse cuando nadie usa la página y no gasta sus horas gratis.

## Configuración

| Variable | Valor |
|---|---|
| `ConnectionStrings__PostgreSQL` | `Host=<host>;Database=<base>;Username=<usuario>;Password=<clave>;SSL Mode=Require`. Formato Npgsql: la cadena `postgresql://` de Neon no sirve tal cual. |
| `Jwt__Key` | La genera Render. |
| `Proxy__ConfiarEnEncabezados` | Viene en la imagen. Hace que el límite anti-abuso use la IP de cada visitante y no la del proxy. |
| `Proxy__EncabezadoIp` | `CF-Connecting-IP`, en `render.yaml`. En Render el `X-Forwarded-For` conserva lo que manda el visitante y se puede falsificar; Cloudflare escribe la IP real en `CF-Connecting-IP`. Comprobado en Render el 2026-11-05: sin este ajuste, 32 intentos con IPs inventadas no se bloqueaban. |
| `PORT` | La pone Render; la API la respeta. |
| `AI__ApiKey`, `AI__Model` | Opcionales, para la explicación con IA. |

## Actualizar

El servicio se creó desde la dirección pública del repositorio, sin conectar la cuenta de GitHub, así que **no se publica solo**. Después de un `git push`, en Render: *Blueprint > Manual sync* si cambió `render.yaml`, y luego en el servicio *Manual Deploy > Deploy latest commit*. Los cambios que se hagan desde el panel de la nube quedan en Neon. La base de la PC y la de la nube son independientes.

## Límites del plan gratuito

- 512 MB de memoria y CPU compartida: alcanza para un curso, no para cientos de personas a la vez.
- Si UptimeRobot falla, la página vuelve a dormirse y la siguiente visita tarda cerca de un minuto.
- Neon gratis suspende la base tras unos minutos sin uso. La primera consulta después tarda alrededor de un segundo más.
