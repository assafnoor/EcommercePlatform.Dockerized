# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN adduser --disabled-password --gecos "" appuser && \
    mkdir -p /app/logs && \
    chown -R appuser:appuser /app && \
    chmod 750 /app/logs
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER appuser
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV DOTNET_RUNNING_IN_CONTAINER=true
# ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

HEALTHCHECK --interval=30s --timeout=10s --start-period=20s --retries=3 \
    CMD curl --fail --silent --show-error http://localhost:8080/health || exit 1

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["products.api/products.api.csproj", "products.api/"]
COPY ["shared/shared.csproj", "shared/"]
RUN dotnet restore "products.api/products.api.csproj"

COPY ["products.api/", "products.api/"]
COPY ["shared/", "shared/"]

WORKDIR /src/products.api
RUN dotnet build "products.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "products.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
ARG BUILD_CONFIGURATION=Release

WORKDIR /app
COPY --from=publish /app/publish .

# COPY migration-bundle and entrypoint.sh
# COPY ["../migration-bundle", "./migration-bundle"]
# RUN chmod +x ./migration-bundle

COPY ["products.api/entrypoint.sh", "./entrypoint.sh"]
RUN chmod +x ./entrypoint.sh

# Set environment based on build configuration

ARG ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT

USER root
RUN chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["./entrypoint.sh"]