# Arquitectura

## Frontend

React, TypeScript, Vite y CSS. Las rutas actuales se resuelven en `App.tsx` y consumen la API mediante servicios. El token JWT se envía en el encabezado `Authorization`.

## Backend

ASP.NET Core Web API organizado en cuatro proyectos:

- `API`: controladores, autenticación, autorización y HTTP.
- `Application`: contratos, interfaces y casos de uso.
- `Domain`: entidades, constantes, estados y banco RIASEC.
- `Infrastructure`: EF Core, PostgreSQL, servicios y migraciones.

## PostgreSQL

`OrientacionVocacionalDbContext` usa Npgsql. Las migraciones crean usuarios, perfiles, preguntas, intentos, respuestas, perfiles RIASEC, instituciones, carreras, compatibilidades e historial.

## JWT

El backend emite JWT con identificador de usuario, correo y rol. Los controladores obtienen el usuario desde `ClaimTypes.NameIdentifier`; no confían en un UserId enviado por React.

## RIASEC

El banco contiene exactamente 60 preguntas. El algoritmo suma las seis dimensiones, aplica desempate fijo y guarda el perfil con versión `RIASEC-60-V1`.

## Motor de reglas

Las compatibilidades se almacenan por carrera y dimensión RIASEC. El servidor combina puntuaciones, dimensión principal/secundaria y nivel de regla para calcular un porcentaje reproducible y una explicación textual. La IA no interviene en este cálculo.

## Catálogo

Instituciones y carreras se almacenan con estado, fuente y fecha de actualización. No se siembran instituciones ni carreras ficticias.

## Filtros y recomendaciones

El backend aplica filtros sobre datos persistidos antes de calcular y ordenar compatibilidades. Cada consulta se registra para el historial del estudiante autenticado.

## IA opcional

`IAiExplanationService` recibe únicamente el perfil RIASEC y recomendaciones estructuradas. Usa `AI__ProviderUrl`, `AI__Model` y `AI__ApiKey` desde configuración del backend. Sin clave o modelo devuelve un mensaje de indisponibilidad y no bloquea recomendaciones.

## Administrador

Las rutas `/api/admin/*` exigen `Authorize(Roles = "ADMINISTRADOR")`. El dashboard, estudiantes, instituciones, carreras y compatibilidades se gestionan desde servicios de aplicación sobre PostgreSQL. Las contraseñas nunca se devuelven.
