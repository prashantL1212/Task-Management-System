# Architecture — Task Management System

Finalized architecture and the decisions behind it. This document is the source of
truth for how the solution is structured; the spec lives in `project-scope.md`.

## Stack

| Concern        | Choice |
|----------------|--------|
| Backend        | .NET 8, ASP.NET Core Web API |
| Architecture   | Clean Architecture (Domain / Application / Infrastructure / API) |
| Data access    | EF Core 8, Code-First + Migrations, Repository + Service pattern |
| Database (dev) | SQL Server LocalDB |
| Database (Docker) | SQL Server 2022 container |
| Auth           | JWT bearer tokens |
| Validation     | FluentValidation |
| Frontend       | React 19 (Vite), Axios, Context API, React Router |
| Testing        | xUnit + Moq + FluentAssertions (unit tests only) |
| Containers     | Docker + Docker Compose (sqlserver + api + frontend) |

## Project graph

```
TMS.Domain          (net8.0)        no dependencies
TMS.Shared          (netstandard2.0) no dependencies
TMS.Application     (net8.0)        -> Domain, Shared
TMS.Infrastructure  (net8.0)        -> Application, Domain
TMS.API             (net8.0)        -> Application, Infrastructure, Shared
TMS.Application.Tests (net8.0)      -> Application, Domain, Shared
```

Dependencies point inward only. The API is the composition root — the single place
where Infrastructure implementations are bound to Application interfaces.

## Locked decisions

1. **Repository + Service** — repositories wrap `DbContext`; services hold business logic.
2. **Enums stored as `int`** — `TaskStatus` (ToDo=0, InProgress=1, Done=2),
   `TaskPriority` (Low=0, Medium=1, High=2, Critical=3).
3. **`Task.AssignedTo` is a plain `string`** — no `AssignedToUserId`, no foreign key.
   The `Users` table is used **only for authentication/authorization** and has **no
   relationship** to `Tasks`. This is intentional, to match the assignment exactly.
4. **Soft delete** — `IsDeleted` flag on `Tasks` plus an EF global query filter so
   deleted rows are invisible to normal queries.
5. **Summary endpoint** uses a **raw SQL query** (`GET /api/tasks/summary`).
6. **JWT** — login issues a signed token; task endpoints are `[Authorize]`-protected.
   Passwords stored hashed (BCrypt).
7. **Frontend** — Vite build, token stored in **localStorage**, attached via an Axios
   interceptor; `ProtectedRoute` guards routes; auth state in Context API.
8. **Testing** — unit tests only, targeting the Application service/validator layer
   with mocked repositories.
9. **Migrations auto-APPLY on startup** in every environment via `db.Database.Migrate()`.
   Note: creating a migration after an entity change is still a manual dev step
   (`dotnet ef migrations add <Name>`); only *applying* pending migrations is automatic.
10. **Seeding is Development-only and idempotent** — a custom `DbSeeder` runs at startup
    only when `IsDevelopment()` and only when tables are empty (admin user + 10+ tasks).
    EF `HasData` is deliberately avoided because it would seed in all environments.

## Runtime note (.NET 8 on a .NET 9 machine)

Projects target `net8.0` per the assignment. The dev machine has the .NET 9 SDK/runtime,
so `RollForward=Major` is set on the API to allow local execution on .NET 9. Docker uses
the real `mcr.microsoft.com/dotnet/aspnet:8.0` image, so the container runs true .NET 8.

## Layout

```
src/
  TMS.Domain/         Entities, Enums, Common
  TMS.Shared/         Models (ApiResponse), Constants, Enums
  TMS.Application/     DTOs, Interfaces (Repositories/Services/Security),
                       Services, Validators, Mappings
  TMS.Infrastructure/  Persistence (DbContext + Configurations), Repositories,
                       Security (JWT, hashing), Seed, Migrations
  TMS.API/             Controllers, Middleware, Extensions, Program.cs
tests/
  TMS.Application.Tests/ Services, Validators, Helpers
frontend/
  src/                 api, auth, components, pages, hooks, utils
```
