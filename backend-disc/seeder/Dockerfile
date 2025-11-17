# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy ONLY the seeder project file first
COPY backend-disc/seeder/seeder.csproj backend-disc/seeder/

# Restore dependencies
RUN dotnet restore backend-disc/seeder/seeder.csproj

# Copy the full project source
COPY backend-disc/seeder/ backend-disc/seeder/

# Publish the seeder
RUN dotnet publish backend-disc/seeder/seeder.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "seeder.dll"]
