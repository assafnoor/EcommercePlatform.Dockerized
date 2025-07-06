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

COPY ["gateway/gateway.csproj", "gateway/"]
RUN dotnet restore "gateway/gateway.csproj"
COPY ["gateway/", "gateway/"]

WORKDIR /src/gateway
RUN dotnet build "gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
ARG BUILD_CONFIGURATION=Release

WORKDIR /app
COPY --from=publish /app/publish .

# اضف متغير بيئة ASPNETCORE_ENVIRONMENT بقيمة افتراضية تعتمد على BUILD_CONFIGURATION
ENV ASPNETCORE_ENVIRONMENT=${BUILD_CONFIGURATION}

USER root
RUN chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "gateway.dll"]
