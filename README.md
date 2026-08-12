# MediBook API

A clinic appointment booking system built with ASP.NET Core and Clean Architecture. Patients book appointments with doctors, doctors manage their availability, and admins oversee the clinic through a live dashboard — with real conflict detection and a proper appointment lifecycle, not just CRUD.

## Why this project

Most portfolio APIs are CRUD wrappers around a single entity. MediBook models an actual business problem: doctors have limited, recurring availability windows, patients can't double-book a slot, and appointments move through a real state machine (`Pending → Confirmed → Completed`, with `Cancelled` reachable from either open state). The interesting part isn't the endpoints — it's the rules behind them.

## Architecture

Built in Clean Architecture, with four projects enforcing a strict one-way dependency rule:

```
Domain  ←  Application  ←  Infrastructure  ←  API
```

- **Domain** — entities, enums, and custom exceptions. Zero external dependencies — no EF Core, no ASP.NET Core. Pure C#.
- **Application** — interfaces, DTOs, and services. Holds all business logic, but only depends on abstractions (`IDoctorRepository`, not `AppDbContext`).
- **Infrastructure** — EF Core, the DbContext, repository implementations, JWT generation. The only layer that knows Postgres exists.
- **API** — thin controllers, middleware, and DI wiring. Translates HTTP requests into service calls and exceptions into HTTP responses.

Dependencies only point inward. `Domain` has no idea `Infrastructure` exists — enforced by project references, not convention.

## Core features

- **JWT authentication** with role-based authorization (`Admin`, `Doctor`, `Patient`)
- **Doctor availability management** — recurring weekly windows per doctor
- **Appointment booking with real validation**:
  - Rejects bookings outside a doctor's declared availability
  - Rejects double-bookings for the same doctor and time slot (conflict detection)
- **Appointment status lifecycle** — a proper state machine, not a free-form status field. Illegal transitions (e.g. completing an appointment that was never confirmed) are rejected.
- **Admin dashboard** — today's appointment count, revenue from completed appointments, busiest doctor
- **Filtering and pagination** on the appointments list (by doctor, status, date range)
- **Global error-handling middleware** — domain exceptions (`BookingConflictException`, `SlotUnavailableException`, `InvalidStatusTransitionException`) are mapped to correct HTTP status codes, not generic 500s
- **FluentValidation** for cross-field rules (e.g. an availability window's end time must be after its start time) alongside Data Annotations for simpler field-level rules
- **Unit tests (xUnit + Moq)** covering booking conflict detection, availability checks, and every status transition — repositories are mocked, so tests run without a real database

## Tech stack

ASP.NET Core 9 · Entity Framework Core · PostgreSQL · JWT Bearer Authentication · FluentValidation · xUnit · Moq · Swagger / OpenAPI

## API overview

| Area | Endpoints |
|---|---|
| Auth | `POST /api/auth/register`, `POST /api/auth/login` |
| Specialties | `GET/POST /api/specialties` |
| Doctors | `GET/POST /api/doctors`, `GET /api/doctors/{id}` |
| Doctor Availability | `GET /api/doctoravailabilities/doctor/{doctorId}`, `POST /api/doctoravailabilities` |
| Patients | `GET/POST /api/patients` |
| Appointments | `GET/POST /api/appointments`, `GET /api/appointments/filter`, `PATCH /api/appointments/{id}/confirm`, `.../cancel`, `.../complete` |
| Dashboard | `GET /api/dashboard` (Admin only) |

Full request/response schemas are available via Swagger once running.

## Running locally

**Prerequisites:** .NET 9 SDK, PostgreSQL

1. Clone the repo
2. Update `MediBook.API/appsettings.json` with your local PostgreSQL connection string
3. Apply migrations:
   ```
   dotnet ef database update --project MediBook.Infrastructure --startup-project MediBook.API
   ```
4. Run the API:
   ```
   dotnet run --project MediBook.API
   ```
5. Open Swagger at `https://localhost:{port}/swagger`

## Running tests

```
dotnet test
```

## What I'd add next

- Migrate patient accounts to real authenticated users (currently patients are just named records, not linked to login accounts — a deliberate simplification for this stage)
- Push filtering and aggregation logic down into the database query layer instead of filtering in-memory, for better performance at scale
- Deploy to Azure (App Service + Azure Database for PostgreSQL) with CI/CD via GitHub Actions
