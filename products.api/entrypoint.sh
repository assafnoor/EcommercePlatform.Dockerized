#!/bin/sh
set -e

#RUN dotnet ef migrations bundle --output /app/migration-bundle --self-contained true --runtime linux-x64
# echo "Running EF migrations bundle..."
# ./migration-bundle

echo "Starting ASP.NET Core application..."
exec dotnet products.api.dll


