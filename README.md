# Task Management System

Full-stack task management: .NET 8 Clean Architecture API + React (Vite) SPA, backed by
SQL Server with EF Core Code-First migrations. See `project-scope.md` for requirements and
`ARCHITECTURE.md` for the design decisions.

> Status: **scaffolded** — solution structure, projects, references, packages, and config
> are in place. Business logic is not yet implemented.

## Prerequisites

- .NET 8 SDK (or newer — projects roll-forward to run on .NET 9)
- Node.js 20+ and npm
- SQL Server LocalDB (for local dev) — ships with Visual Studio / SQL Server Express
- Docker Desktop (optional, for the containerized stack)

## Solution layout

```
src/   TMS.Domain · TMS.Application · TMS.Infrastructure · TMS.API · TMS.Shared
tests/ TMS.Application.Tests
frontend/  React (Vite) SPA
```

## Run locally

### Backend
```bash
dotnet build TaskManagementSystem.sln
dotnet run --project src/TMS.API
```
The API applies EF migrations on startup and (in Development) seeds an admin user and
sample tasks. Swagger UI is served at the API root.

### Frontend
```bash
cd frontend
npm install
npm run dev
```

## Run with Docker

```bash
docker compose up --build
```
Brings up three services:

| Service   | URL / Port |
|-----------|------------|
| frontend  | http://localhost:3000 |
| api       | http://localhost:8080 |
| sqlserver | localhost:1433 |

`docker-compose.override.yml` runs the API in the Development environment so migrations
apply and the seeder populates demo data inside the container.

## Tests

```bash
dotnet test
```

## EF migrations

```bash
# after changing an entity, create a migration (manual step):
dotnet ef migrations add <Name> \
  --project src/TMS.Infrastructure \
  --startup-project src/TMS.API

# applying is automatic on app startup; to apply manually:
dotnet ef database update \
  --project src/TMS.Infrastructure \
  --startup-project src/TMS.API
```
