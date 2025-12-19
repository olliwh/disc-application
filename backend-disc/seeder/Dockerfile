FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY backend-disc/seeder/seeder.csproj backend-disc/seeder/
COPY backend-disc/class-library-disc/class-library-disc.csproj backend-disc/class-library-disc/

RUN dotnet restore backend-disc/seeder/seeder.csproj

COPY backend-disc/seeder/ backend-disc/seeder/
COPY backend-disc/class-library-disc/ backend-disc/class-library-disc/

RUN dotnet publish backend-disc/seeder/seeder.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "seeder.dll"]