# Task Management System

Full-stack task management: .NET 8 Clean Architecture API + React (Vite) SPA, backed by
SQL Server with EF Core Code-First migrations. See `project-scope.md` for requirements and
`ARCHITECTURE.md` for the design decisions.

Features a full task CRUD API (filtering, soft-delete, and a raw-SQL summary endpoint),
JWT authentication, FluentValidation, a global exception handler, and a React dashboard with
create/edit/delete, filtering, and priority colour-coding.

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

### Run both simultaneously (recommended)
```bash
cd frontend
npm install        # first time only — installs frontend deps (including 'concurrently')
npm run dev:all    # starts the API and the React app together
```

Once running:

| App          | URL                          |
|--------------|------------------------------|
| Frontend SPA | http://localhost:5173        |
| API / Swagger| http://localhost:5210/swagger|

**Default login:** `admin` / `Admin@123` (seeded automatically in Development).

### Terminate the application
The app started by `npm run dev:all` runs two processes (API + React).
Press `Ctrl+C` twice in the terminal to stop both.

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
apply and the seeder populates demo data inside the container. Log in with the same default
credentials: `admin` / `Admin@123`.

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
