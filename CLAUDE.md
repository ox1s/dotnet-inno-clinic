# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

InnoClinic — a .NET 10 / Aspire-orchestrated microservice backend (`inno-clinic-backend/`), a React 19 + Vite SPA (`inno-clinic-frontend/`), and a Python Telegram bot (`inno-clinic-backend/InnoClinic.TelegramBot/`). `Requirments.md` holds the user-story spec (US-1 … US-68); `таски_todo.md` tracks per-story backend/UI implementation status and is the fastest way to see what is actually done vs. stubbed.

## Commands

Run everything (starts all services, containers, and the SPA; requires Docker):

```powershell
aspire run                      # from repo root — aspire.config.json points at InnoClinic.AppHost
# or
dotnet run --project inno-clinic-backend/InnoClinic.AppHost
```

Backend build & test (note: `.slnx`, not `.sln`):

```powershell
dotnet build inno-clinic-backend/InnoClinic.slnx
dotnet test  inno-clinic-backend/InnoClinic.slnx

# single test project
dotnet test inno-clinic-backend/InnoClinic.Profile/tests/Profile.Features.UnitTests

# single test / class
dotnet test <test-project> --filter "FullyQualifiedName~EditDoctorStatusByBotCommandHandlerTests"
dotnet test <test-project> --filter "FullyQualifiedName~Handle_ShouldUpdateDoctorStatus_WhenDoctorExists"
```

`InnoClinic.IntegrationTests` uses `DistributedApplicationTestingBuilder` — it boots the **entire** AppHost (Postgres, RabbitMQ, Mongo, Minio, Azurite containers) per test. Don't run it as part of a quick loop; it needs Docker and ~30s+ per test.

Frontend (`cd inno-clinic-frontend`):

```powershell
npm run dev; npm run build; npm run lint; npm run typecheck; npm run format
```

EF Core migrations — each service owns its own DbContext and migrations folder:

```powershell
# ClinicManagement (single-project service)
dotnet ef migrations add <Name> -p inno-clinic-backend/InnoClinic.ClinicManagement/src/ClinicManagement.Api

# Appointment (migrations live under Data/Migrations)
dotnet ef migrations add <Name> -p inno-clinic-backend/InnoClinic.Appointment/src/Appointment.Api -o Data/Migrations

# Identity / Profile (layered: migrations in Infrastructure, startup is the Api)
dotnet ef migrations add <Name> `
  -p inno-clinic-backend/InnoClinic.Identity/src/Identity.Infrastructure `
  -s inno-clinic-backend/InnoClinic.Identity/src/Identity.Api
```

Migrations are applied automatically at startup (`ApplyMigrations()` / `Database.MigrateAsync()` in each `Program.cs`), so a plain `aspire run` brings the schema up to date.

## Architecture

### Orchestration is the source of truth

`InnoClinic.AppHost/AppHost.cs` is the single place that wires services, containers, connection strings, and startup ordering. Anything about "which service talks to what" is answered there, not in appsettings. Resource names in that file (`innoclinic-database`, `rabbitmq`, `notifications-db`, `blobs`, `minio`, `mailpit`) are the connection-string keys services read via `GetConnectionString(...)` / `builder.AddXxxClient(...)`. Adding a dependency means adding both `.WithReference()` and `.WaitFor()`.

Infrastructure spun up: Postgres (host port 5435, pgAdmin, persistent volume), MongoDB + Mongo Express, RabbitMQ + management plugin, Mailpit (SMTP), Azurite (Azure Blob emulator), Minio (9000 API / 9001 console, bind-mounted `minio-data/`). Secrets `botApiKey` and `telegramToken` come from `InnoClinic.AppHost/.env` as `Parameters__*`.

### One database, one schema per service

All relational services share the **same** Postgres database (`innoclinic-database`) and isolate themselves by `modelBuilder.HasDefaultSchema(...)`: `identity`, `profile`, `clinic_management`, `appointment`. This is why `view_creation.md` can define `appointment.appointments_view` with joins across `profile.doctors`, `identity.accounts`, and `clinic_management."Services"` — that view backs `AppointmentView`/receptionist appointment listing and must be created manually (or via the `AddAppointmentsView` migration) when schemas change. Beware: entity tables use inconsistent casing (`profile.doctors` snake_case vs `clinic_management."Services"` PascalCase).

### Gateway and auth

`Gateway.Api` is a YARP reverse proxy configured purely from `appsettings.json`, using Aspire service discovery (`https+http://<resource-name>`) and stripping a path prefix per service: `/identity`, `/profile`, `/clinic-management`, `/appointment`. The SPA talks only to the gateway (`VITE_API_BASE_URL`).

Identity issues JWTs; **every** other service validates them independently with its own copy of `JwtSettings` (`Secret`/`Issuer`/`Audience`) in appsettings — change the secret in one place and you must change it everywhere. Roles are the three constants in `InnoClinic.Shared/Roles.cs` (`Patient`, `Doctor`, `Receptionist`).

Authorization style differs per service, deliberately:
- Most endpoints: `.RequireAuthorization(policy => policy.RequireRole(Roles.X))`.
- ClinicManagement adds a permission indirection: `Permissions` constants → `RolePermissionMapping.Map` (role → permission set) → `PermissionAuthorizationRequirement`, used as `.RequirePermission(Permissions.OfficesManipulate)`. New ClinicManagement endpoints should go through `RequirePermission`, and a new permission must be added to `RolePermissionMapping` or nobody can call it.
- Profile has an extra `"BotPolicy"` (`BotApiKeyHandler` / `BotApiKeyRequirement`) with authentication schemes cleared, for the Telegram bot's API-key calls.

### Messaging — two coexisting stacks

- **Profile** publishes with **Wolverine** over RabbitMQ to exchanges `doctor-created-events`, `notifications`, `telegram-account-linked-events`, and dispatches its own commands in-process via `IMessageBus.InvokeAsync(command)` (handlers discovered from `Profile.Features`/`Profile.Infrastructure` assemblies — new handler assemblies must be added to `opts.Discovery.IncludeAssembly`).
- **Identity** and **Notification** use the raw `RabbitMQ.Client` API: Identity publishes to the `email-verification-queue` queue directly; Notification runs one `BackgroundService` consumer per message type (`EmailVerificationConsumer`, `DoctorCreatedConsumer`, `SendDailyPollCommandConsumer`, `TelegramAccountLinkedConsumer`), each declaring its own queue and nacking-with-requeue on failure.

Message contracts live in `InnoClinic.Shared/DTOs/` and are shared by project reference, so changing one is a breaking change for both the publisher and consumer.

Synchronous cross-service calls go through typed gateway interfaces (`Appointment.Api/External/IProfileGateway`, `IOfficeGateway`, `IServiceGateway`) resolved by Aspire service discovery + `IProfileService` in Identity (with a `FakeProfileService` alternative for local work).

### Four different intra-service styles

Match the style of the service you're editing rather than normalizing across them:

| Service | Style |
|---|---|
| **Identity** | Full clean architecture: `Domain` (aggregates, value objects, `*Errors` static classes) / `Application` (MediatR commands+queries, `ErrorOr<T>`, FluentValidation `ValidationBehavior`, `LoggingBehavior`) / `Infrastructure` / `Contracts` / `Api` (MVC controllers deriving from `ApiController`, which maps `List<Error>` → ProblemDetails). |
| **Profile** | `Domain` / `Features` (Wolverine command handlers) / `Infrastructure` (repositories, `SoftDeleteInterceptor`, Quartz `EmailReminderJob`) / `Api` (minimal-API `IEndpoint` classes that often query `ProfileDbContext` inline). |
| **ClinicManagement** | Single project, vertical slices under `Features/<Aggregate>/<UseCase>.cs`. One file = one internal sealed use-case class containing nested `Request`/`Response` records, a `Handle` method, a `Validator : AbstractValidator<Request>`, and an `Endpoint : IEndpoint`. Use cases are registered by hand in `Program.cs` (`AddScoped<CreateOffice>()`) — **easy to forget**. Errors surface via `GlobalExceptionHandler` / `ValidationExceptionHandler` + ProblemDetails. |
| **Appointment** | Single project, `Features/<Actor>/<UseCase>/` with separate `*Endpoint`/`*Handler`/`*Request`/`*Response`/`*Validator` files, a hand-rolled `Result<T>`/`Error` type (not `ErrorOr`), and static handler methods referenced directly from `MapPost(...)`. |

The `IEndpoint` + `AddEndpoints()`/`MapEndpoints()` assembly-scanning pattern is duplicated per service (ClinicManagement, Profile, Appointment each have their own copy) — endpoints are auto-discovered, so a new `IEndpoint` implementation needs no registration, but its use-case dependency usually does.

### Blob storage (currently in flux — branch `features/storages`)

`IBlobService` (upload/download/delete by `Guid` file id) has one full implementation, `AzureBlobService` (Azurite, container `files`), which is what `Program.cs` binds to `IBlobService`. `MinioBlobService` is registered as a concrete type, implements only bucket creation + upload (bucket `innoclinic-files`), and is used by `DbInitializer` to create the bucket at startup. Photos are stored as the blob `Guid` string inside the `Photo` value object.

### Observability

`InnoClinic.ServiceDefaults` provides OpenTelemetry (ASP.NET Core, HttpClient, EF Core, Npgsql), health checks (`/health`, `/alive`), service discovery, and standard HTTP resilience. Note it is **not** applied uniformly — only Gateway, Identity, and Appointment call `AddServiceDefaults()`; ClinicManagement and Profile configure telemetry/health themselves. Identity, Profile, and Notification use Serilog configured from appsettings.

## Conventions

- **Central package management**: all versions live in `inno-clinic-backend/Directory.Packages.props`. `.csproj` files carry bare `<PackageReference Include="..." />` with no `Version` — add the version to `Directory.Packages.props` instead.
- `SonarAnalyzer.CSharp` runs on most projects; the build currently emits ~98 warnings and 0 errors. Don't treat the pre-existing warnings as your regression, but don't add new ones.
- `.editorconfig` (repo root, 4-space C#, CRLF) sets `dotnet_separate_import_directive_groups = true` — hence the blank-line-separated `using` groups seen throughout. Preserve that grouping.
- Tests: xUnit + FluentAssertions, EF Core InMemory provider with a fresh `Guid`-named database per fixture, `Arrange/Act/Assert` comments. Both Moq and NSubstitute are available; check the neighbouring test project before picking one.
- `.http` request files live under each service's `requests/` folder and are linked into the API project; Postman collections are in `postman/`.
- `.gitignore` deliberately excludes `inno-clinic-backend/**/*Features/**/*.md` and `*.png`, so design notes under `Features/!_documents/` are local-only and won't show in git.
