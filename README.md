# DISC Application

Full-stack application with .NET backend, React frontend, and multiple databases (MSSQL, MongoDB, Neo4j).

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running
- [Git](https://git-scm.com/downloads)
- (Optional) [Node.js 18+](https://nodejs.org/) for local frontend development
- (Optional) [.NET 8 SDK](https://dotnet.microsoft.com/download) for local backend development

## Quick Start

### 1. Clone the Repository

```bash
git clone <repository-url>
cd disc-application
```

### 2. Set Up Environment Variables

Create a `.env` file in the root directory:

```bash
# mssql Configuration
DB_USER=sa
DB_PASSWORD=YourPassword
DB_NAME=disc_profile_relational_db
DB_HOST=mssql
DB_PORT=1433
ConnectionStrings__DefaultConnection=Server=${DB_HOST},${DB_PORT};Database=${DB_NAME};User Id=${DB_USER};Password=${DB_PASSWORD};TrustServerCertificate=True;

#Mongo Configuration
MONGO_PASSWORD=YourMongoPassword
MONGO_USER=root
ConnectionStrings__MongoConnection=mongodb://${MONGO_USER}:${MONGO_PASSWORD}@mongo:27017

# Neo4j Configuration
NEO4J_PASSWORD=YourNeo4jPassword
NEO4J_USER=neo4j
NEO4J_HOST=neo4jdb
NEO4J_PORT=7687
ConnectionStrings__Neo4jConnection=bolt://${NEO4J_HOST}:${NEO4J_PORT}


# API Configuration
API_SECRET_KEY=YourSuperSecretKeyForJWTTokensMinimum32Characters


# Frontend Configuration
VITE_API_URL="http://localhost:5000/api"
```

**Important:** Replace all placeholder passwords and keys with your own secure values! Never commit the `.env` file to version control.

### 3. Start the Application

# Start all services (without seeder)

docker compose up -d

# Or start with seeder to populate initial data

docker compose up -d --scale seeder=1

### 4. Access the Application

- **Frontend:** http://localhost:3000
- **Backend API:** http://localhost:5000/api

## Development

### Frontend Development

cd frontend-disc
npm install
npm run dev

### Backend Development

cd backend-disc/backend-disc
dotnet restore
dotnet run

### Running the Seeder

To seed the database with initial data:

docker compose up seeder

# Stop all services

docker compose down

# Rebuild services

docker compose up -d --build

# Frontend Login

username: alice, password: Pass@word1
username: admin, password: Pass@word1
