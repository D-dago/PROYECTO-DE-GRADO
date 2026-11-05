# Publicar en Google Cloud Run con la base en Neon

El sistema corre en un contenedor de **Google Cloud Run** y guarda los datos en una base PostgreSQL de **Neon**. Cloud Run se apaga cuando nadie lo usa y vuelve a arrancar en unos segundos. Ambos son gratuitos dentro de su cuota.

## Qué se publica

- El `Dockerfile` de la raíz compila la web y la API en una sola imagen. La API sirve la web, igual que en la PC.
- La base de la nube **no es la de la PC**. Lleva el catálogo (instituciones, carreras, reglas y preguntas) y dos cuentas: el administrador y el estudiante de prueba. Los estudiantes reales quedan solo en la PC.
- El respaldo que se sube es `tools/respaldos/para-la-nube-2026-11-05.dump`. No se sube a git.

## Lo que hace el dueño de las cuentas

1. **Google Cloud:** crear una cuenta en <https://console.cloud.google.com>, crear un proyecto y activar la facturación. Pide tarjeta, aunque el uso de esta aplicación entra en la cuota gratuita. Conviene crear una alerta de presupuesto de 1 USD en *Facturación > Presupuestos y alertas*.
2. **Neon:** crear una cuenta en <https://neon.tech> y un proyecto en la región **AWS São Paulo (sa-east-1)**, la más cercana a Bolivia. Copiar la cadena de conexión (*Connection string*) y guardarla en un archivo `neon.local.txt` en la raíz del proyecto. Ese archivo no se sube a git.
3. **Iniciar sesión en Google desde la PC:** con el Google Cloud CLI instalado, ejecutar `gcloud auth login` y elegir la cuenta.

## Lo que se hace después (puede hacerlo Claude)

```bash
# 1. Cargar la base en Neon (la cadena de conexión sale de neon.local.txt)
pg_restore --no-owner --no-privileges -d "<cadena de Neon>" tools/respaldos/para-la-nube-2026-11-05.dump

# 2. Publicar desde la carpeta del proyecto
gcloud run deploy orientacion-vocacional --source . --region southamerica-east1 \
  --allow-unauthenticated --min-instances 0 --max-instances 1 --memory 512Mi \
  --set-env-vars "ConnectionStrings__PostgreSQL=<cadena Npgsql de Neon>,Jwt__Key=<clave nueva>,Jwt__Issuer=OrientacionVocacional.API,Jwt__Audience=OrientacionVocacional.Frontend"
```

`--max-instances 1` no es solo para ahorrar. La actualización en vivo y el límite anti-abuso guardan su estado en la memoria del servidor, así que con dos copias del servidor una pantalla podría no enterarse de un cambio.

## Configuración

| Variable | Valor |
|---|---|
| `ConnectionStrings__PostgreSQL` | `Host=...;Database=...;Username=...;Password=...;SSL Mode=Require` (formato Npgsql, no `postgres://`) |
| `Jwt__Key` | Clave nueva de al menos 32 bytes; no reutilizar la de la PC |
| `Proxy__ConfiarEnEncabezados` | Ya viene en la imagen. Hace que el límite anti-abuso use la IP real de cada visitante y no la del proxy de Google |
| `PORT` | La pone Cloud Run; la API la respeta |
| `AI__ApiKey`, `AI__Model` | Opcionales, para la explicación con IA |

## Actualizar después de un cambio

Volver a ejecutar el mismo `gcloud run deploy`. Los cambios de catálogo hechos desde el panel de la nube quedan en Neon. La base de la PC y la de la nube son independientes.
