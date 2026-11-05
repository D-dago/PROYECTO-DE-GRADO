# Imagen para publicar el sistema en Google Cloud Run (o cualquier hosting de contenedores).
# Una sola imagen sirve la API y la web compilada, igual que en la PC.

# 1. Web (React): se compila dentro de la carpeta wwwroot de la API, como en el proyecto.
FROM node:22-alpine AS web
WORKDIR /src/frontend
COPY frontend/package.json frontend/package-lock.json ./
RUN npm ci
COPY frontend/ ./
RUN npm run build

# 2. API (.NET)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS api
WORKDIR /src/backend
COPY backend/ ./
RUN dotnet publish OrientacionVocacional.API/OrientacionVocacional.API.csproj -c Release -o /app
COPY --from=web /src/backend/OrientacionVocacional.API/wwwroot /app/wwwroot

# 3. Imagen final, solo con lo necesario para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:10.0
# QuestPDF (exportar resultados a PDF) necesita fontconfig y al menos una fuente en Linux.
RUN apt-get update \
    && apt-get install -y --no-install-recommends libfontconfig1 fonts-dejavu-core \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=api /app ./
ENV ASPNETCORE_ENVIRONMENT=Production \
    Proxy__ConfiarEnEncabezados=true
USER $APP_UID
ENTRYPOINT ["dotnet", "OrientacionVocacional.API.dll"]
