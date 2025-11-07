
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution and project files (adjusted paths)
COPY backend-disc/backend-disc.sln ./        
COPY backend-disc/backend-disc/backend-disc.csproj backend-disc/
COPY backend-disc/class-library-disc/class-library-disc.csproj class-library-disc/

# Restore dependencies
RUN dotnet restore backend-disc/backend-disc.csproj

# Copy all source code
COPY backend-disc/backend-disc/ backend-disc/
COPY backend-disc/class-library-disc/ class-library-disc/

RUN echo "=== Files under /src/backend-disc ===" && ls -R /src/backend-disc
RUN echo "=== Files under /src/class-librarConnectionStrings__DefaultConnectiony-disc ===" && ls -R /src/class-library-disc

# Build and publish
RUN dotnet publish backend-disc/backend-disc.csproj -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "backend-disc.dll"]
