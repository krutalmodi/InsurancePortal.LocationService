# Insurance Portal - Location Service Architecture & Design

## Project Overview

**Location Service** is a microservice for the Insurance Portal that owns location data (addresses) and their lookup data (location types, countries, and provinces). Quote Service consumes it over HTTP.

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Domain Model](#domain-model)
3. [Database Design](#database-design)
4. [API Endpoints](#api-endpoints)
5. [Integration with Quote Service](#integration-with-quote-service)
6. [Project Structure](#project-structure)
7. [Configuration](#configuration)

---

## Architecture Overview

- **Stack**: ASP.NET Core (.NET 10), Entity Framework Core with SQL Server, JWT Bearer authentication.
- **Pattern**: Clean Architecture layering, matching Quote Service (Domain, Application, Infrastructure, API).
- **CQRS**: Writes and reads are separated in the Application layer. Commands (`CreateLocationCommand`, `UpdateLocationCommand`, `DeleteLocationCommand`) are handled by `ICommandHandler<TCommand, TResult>` implementations in `Application/Commands`. Queries (`GetLocationByIdQuery`, `GetAllLocationsQuery`) are handled by `IQueryHandler<TQuery, TResult>` implementations in `Application/Queries`. Handlers are hand-rolled with no mediator library. The controller injects the handlers directly, and they are registered in `Program.cs`. Shared request/entity mapping lives in `Application/Mappings/LocationMapper`.
- **Repositories** live in `Infrastructure/Repositories`.
- **Startup** follows the Quote Service `Program.cs` pattern.

### Ownership

- Location Service owns `Location`, `LocationTypeLookup`, `CountryLookup`, and `ProvinceLookup`, including persistence, EF configurations, seed data, and migrations.
- Quote Service does not persist any of these tables.

---

## Domain Model

- **Location**: address record (UnitNumber, StreetAddress1, StreetAddress2, City, PostalCode, ProvinceId, CountryId, LocationTypeId). Locations are unique by these address fields: creating a duplicate returns the existing record ID. Matching is exact (case and whitespace sensitive).
- **LocationTypeLookup**: Residential, Commercial.
- **CountryLookup**: United States, Canada.
- **ProvinceLookup**: US states and Canadian provinces, each linked to a country.

---

## Database Design

- Initial schema is created by the `InitialCreate` migration in `InsurancePortal.LocationService.Infrastructure/Migrations`.
- Lookup tables use integer IDs (`BaseLookup.Id` is `int`), so lookup seed rows must use integer IDs.
- Lookup seed rows must supply all required properties (`Code`, `Name`, `CreatedAt`, `UpdatedAt`).
- Any change to seed data or the model requires a new migration. Otherwise `Database.Migrate()` fails with `PendingModelChangesWarning`.

### Resetting the Database and Migrations (Development)

1. Delete the `LocationService` database and the existing migration files.
2. Run the following from a terminal in the solution folder (`D:\Learning\OnPrem Apps\InsurancePortal\src\InsurancePortal.LocationService`):

```powershell
dotnet ef migrations add InitialCreate --project InsurancePortal.LocationService.Infrastructure --startup-project InsurancePortal.LocationService.API --output-dir Migrations
dotnet ef database update --project InsurancePortal.LocationService.Infrastructure --startup-project InsurancePortal.LocationService.API
```

- The Package Manager Console equivalents are `Add-Migration InitialCreate -OutputDir Migrations` and `Update-Database`. Set the Default project to `InsurancePortal.LocationService.Infrastructure` first.
- `Add-Migration` is not available in a regular PowerShell session.

---

## API Endpoints

All endpoints are under `api/locations` and require a bearer token (`[Authorize]`).

- `POST api/locations`: creates a location (returns 201 Created). If a location with the same address fields (type, unit number, street addresses, city, postal code, province, country) already exists, no new record is created and the existing location is returned. Handled by `CreateLocationCommandHandler`.
- `GET api/locations/{id}`: returns a location by ID, or 404. Handled by `GetLocationByIdQueryHandler`.
- `GET api/locations`: returns all locations. Handled by `GetAllLocationsQueryHandler`.
- `PUT api/locations/{id}`: updates a location, or 404. Handled by `UpdateLocationCommandHandler`.
- `DELETE api/locations/{id}`: deletes a location (204), or 404. Handled by `DeleteLocationCommandHandler`.

Note: Quote Service only uses POST and GET by ID.

---

## Integration with Quote Service

- Quote Service calls Location Service through `ILocationClient` (implemented by `LocationClient`, a typed `HttpClient`).
- The caller's bearer token is forwarded on every call.
- Quote Service stores a snapshot of the address fields on `HomeQuote`, `AutoQuote`, and `HealthQuote`, so historical quotes are unaffected by later location changes.

---

## Project Structure

```
InsurancePortal.LocationService/
├── InsurancePortal.LocationService.Domain/
│   ├── Entities/          (Location, BaseLookup)
│   │   └── Lookups/       (LocationTypeLookup, CountryLookup, ProvinceLookup)
│   └── Enums/             (LocationTypeEnum)
├── InsurancePortal.LocationService.Application/
│   ├── Abstractions/      (ICommandHandler, IQueryHandler)
│   ├── Commands/          (CreateLocationCommand, UpdateLocationCommand, DeleteLocationCommand, and their handlers)
│   ├── DTOs/              (LocationRequest, LocationResponse)
│   ├── Mappings/          (LocationMapper)
│   ├── Queries/           (GetLocationByIdQuery, GetAllLocationsQuery, and their handlers)
│   └── Repositories/      (ILocationRepository)
├── InsurancePortal.LocationService.Infrastructure/
│   ├── Data/              (LocationDbContext, Configurations/)
│   ├── Migrations/        (InitialCreate)
│   └── Repositories/      (LocationRepository: CRUD and GetByLocationAsync)
└── InsurancePortal.LocationService.API/
    └── Controllers/       (LocationsController)
```

Template leftovers still present and to be removed: `API/Controllers/WeatherForecastController.cs`, `API/WeatherForecast.cs`, and `Infrastructure/Class1.cs`.

---

## Configuration

- `ConnectionStrings:DefaultConnection`: SQL Server connection. The database is created and migrated automatically at startup via `Database.Migrate()`.
- Swagger UI (`/swagger`) is enabled in the Development environment and uses the Swashbuckle packages.
- Middleware pipeline mirrors Quote Service: Swagger (Development only), CORS (`AllowFrontend` policy), HTTPS redirection, authentication, authorization, controllers.
- `Jwt:Issuer`, `Jwt:Audience`, and `Jwt:Secret` (HMAC signing key). Issuer, audience, and lifetime are validated. The secret is currently a development placeholder.
- Development URL: `http://localhost:5159` (matches the launch profile).
- Quote Service reads this URL from `LocationService:BaseUrl`.

---

## Open Items

- Replace the placeholder JWT signing key with a managed secret.
- Confirm the JWT settings match Identity Service.
