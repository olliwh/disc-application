# DISC Application

Full-stack application with .NET backend, React frontend, and multiple databases (MSSQL, MongoDB, Neo4j).

## Prerequisites

-   [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running
-   [Git](https://git-scm.com/downloads)
-   (Optional) [Node.js 18+](https://nodejs.org/) for local frontend development
-   (Optional) [.NET 8 SDK](https://dotnet.microsoft.com/download) for local backend development

## Quick Start

### 1. Clone the Repository

```bash
git clone <repository-url>
cd disc-application
```

### 2. Set Up Environment Variables

Create a `.env` file in the root directory:

```bash
# Database Passwords
DB_PASSWORD=YourStrong!Passw0rd
MONGO_PASSWORD=YourMongoPassword123
NEO4J_PASSWORD=YourNeo4jPassword123

# Neo4j Configuration
NEO4J_USER=neo4j

# API Configuration
API_SECRET_KEY=YourSuperSecretKeyForJWTTokensMinimum32Characters

# Connection Strings (used by backend)
# MSSQL
ConnectionStrings__DefaultConnection=Server=mssql,1433;Database=DiscDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;

# MongoDB
ConnectionStrings__MongoConnection=mongodb://root:YourMongoPassword123@mongo:27017

# Neo4j
ConnectionStrings__Neo4jConnection=bolt://neo4jdb:7687
```

**Important:** Replace the passwords with your own secure passwords!

### 3. Start the Application

```bash
# Start all services (without seeder)
docker compose up -d

# Or start with seeder to populate initial data
docker compose up -d --scale seeder=1
```

### 4. Access the Application

-   **Frontend:** http://localhost:3000
-   **Backend API:** http://localhost:5000/api
-   **Neo4j Browser:** http://localhost:7474 (login with NEO4J_USER and NEO4J_PASSWORD)
-   **MSSQL:** localhost:1433
-   **MongoDB:** localhost:27018

## Development

### Frontend Development

```bash
cd frontend-disc
npm install
npm run dev
```

### Backend Development

```bash
cd backend-disc/backend-disc
dotnet restore
dotnet run
```

### Running the Seeder

To seed the database with initial data:

```bash
docker compose up seeder
```

## Docker Commands

```bash
# Stop all services
docker compose down

# Stop and remove volumes (clean slate)
docker compose down -v

# View logs
docker compose logs -f

# View specific service logs
docker compose logs -f backend-disc

# Rebuild services
docker compose up -d --build

# Check service status
docker compose ps
```

## Environment Variables Reference

| Variable         | Description                   | Example                |
| ---------------- | ----------------------------- | ---------------------- |
| `DB_PASSWORD`    | MSSQL SA password             | `YourStrong!Passw0rd`  |
| `MONGO_PASSWORD` | MongoDB root password         | `YourMongoPassword123` |
| `NEO4J_PASSWORD` | Neo4j password                | `YourNeo4jPassword123` |
| `NEO4J_USER`     | Neo4j username                | `neo4j`                |
| `API_SECRET_KEY` | JWT secret key (min 32 chars) | `YourSecretKey...`     |

### Frontend Login

username: alice, password: Pass@word1
username: admin, password: Pass@word1
