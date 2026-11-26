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
8. [NuGet Packages](#nuget-packages)

---

## Architecture Overview

- **Stack**: ASP.NET Core (.NET 10), Entity Framework Core with SQL Server, JWT Bearer authentication.
- **Pattern**: Clean Architecture layering, matching Quote Service (Domain, Application, Infrastructure, API).
Shared request/entity mapping lives in `Application/Mappings/LocationMapper`. Commands carry the acting user ID: `CreateLocationCommand(LocationRequest Request, string? UserId)`, `UpdateLocationCommand(Guid Id, LocationRequest Request, string? UserId)`, `DeleteLocationCommand(Guid Id, string? UserId)`. `LocationsController` reads it from the JWT `ClaimTypes.NameIdentifier` claim via the `CurrentUserId` property.
- **Repositories** live in `Infrastructure/Repositories`.
- **Startup** follows the Quote Service `Program.cs` pattern. `Program.cs` registers, in order: controllers, OpenAPI (`AddOpenApi`), CORS policy `AllowFrontend` (any origin, method, header), `AddEndpointsApiExplorer`, `AddSwaggerGen`, `LocationDbContext` (SQL Server, `DefaultConnection`), then scoped services:
  - `ILocationRepository` -> `LocationRepository`
  - `ICommandHandler<CreateLocationCommand, LocationResponse>` -> `CreateLocationCommandHandler`
  - `ICommandHandler<UpdateLocationCommand, LocationResponse?>` -> `UpdateLocationCommandHandler`
  - `ICommandHandler<DeleteLocationCommand, bool>` -> `DeleteLocationCommandHandler`
  - `IQueryHandler<GetLocationByIdQuery, LocationResponse?>` -> `GetLocationByIdQueryHandler`
  - `IQueryHandler<GetAllLocationsQuery, IEnumerable<LocationResponse>>` -> `GetAllLocationsQueryHandler`
  - `ILookupRepository` -> `LookupRepository`, plus the countries, provinces-by-country, and location-types query handlers.
  Then JWT bearer authentication and `AddAuthorization()`.

### Ownership

- Location Service owns `Location`, `LocationTypeLookup`, `CountryLookup`, and `ProvinceLookup`, including persistence, EF configurations, seed data, and migrations.
- Quote Service does not persist any of these tables.

---

## Domain Model

address record (UnitNumber, StreetAddress1, StreetAddress2, City, PostalCode, ProvinceId, CountryId, LocationTypeId). `Id` is a `Guid`. Audit fields: `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy` (`CreatedAt`/`UpdatedAt` default to `DateTime.UtcNow`; `CreatedBy`/`UpdatedBy` are nullable strings holding the caller's user ID). `GetFullAddress()` joins UnitNumber, StreetAddress1, and StreetAddress2 with `, `.
- **BaseLookup** (abstract): `Id` (int), `Code`, `Name`, `Description` (optional, max 1000), `IsActive` (default true), `SortOrder` (int), and the audit fields `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`.
The `LocationTypeEnum` enum (Residential, Commercial) lives in `Domain/Enums`.
- **CountryLookup** (extends BaseLookup): adds `IsoCode` (ISO 3166-1 alpha-2, unique). Seeded: United States (Id 1, `US`) and Canada (Id 2, `CA`), with `SortOrder` 1 and 2.
- **ProvinceLookup** (extends BaseLookup): adds `CountryId` and `ProvinceCode` (unique per country). Seeded: California, Texas, New York, Florida (US, Ids 1-4); Ontario, Quebec, British Columbia, Alberta, Manitoba (Canada, Ids 5-9). `Code` and `ProvinceCode` both hold the province code, `SortOrder` equals Id.

---

## Database Design

- Initial schema is created by the `InitialCreate` migration in `InsurancePortal.LocationService.Infrastructure/Migrations`.
- Lookup tables use integer IDs (`BaseLookup.Id` is `int`), so lookup seed rows must use integer IDs.
- Lookup seed rows must supply all required properties (`Code`, `Name`, `CreatedAt`, `UpdatedAt`).
- Any change to seed data or the model requires a new migration. Otherwise `Database.Migrate()` fails with `PendingModelChangesWarning`.
- Tables: `Locations`, `LocationTypeLookups` (Code unique, seeded Residential Id 1 and Commercial Id 2), `CountryLookups` (Code and IsoCode unique), `ProvinceLookups` (Code unique, unique index on CountryId + ProvinceCode).
- Lookup seed dates are `2025-01-01 00:00:00 UTC`.
- `Locations` has indexes `IX_Locations_ProvinceCity_PostalCode` (ProvinceId, City, PostalCode) and `IX_Locations_CountryId`. Foreign keys to LocationTypeLookups, ProvinceLookups, and CountryLookups use `DeleteBehavior.Restrict`.
- Column sizes: `Locations.UnitNumber` 50, `StreetAddress1` 256 (required), `StreetAddress2` 256, `City` 100 (required), `PostalCode` 20 (required), `CreatedBy`/`UpdatedBy` nvarchar(256). Lookup `CreatedBy`/`UpdatedBy` are unconstrained (nvarchar(max)), as in the InitialCreate migration.
- `CreatedAt`/`UpdatedAt` use `datetime2` with `GETUTCDATE()` defaults on `Locations`.

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

### UI lookups (guest and registered users)

These read-only endpoints allow anonymous requests; registered users can call the same routes with a bearer token. Responses are JSON arrays of active values, ordered by `sortOrder`, then name and ID. Inactive values are retained for historical records but excluded from the UI lists; provinces of inactive countries are also excluded. No audit fields are returned.

- `GET api/lookups/countries`: countries with `id`, `code`, `name`, `description` (nullable), and `isoCode`.
- `GET api/lookups/countries/{countryId}/provinces`: provinces/states with `id`, `code`, `name`, `description` (nullable), `countryId`, and `provinceCode`. A positive unknown or inactive country ID returns `200` with `[]`; a non-positive integer returns `400`. A non-integer route value does not match the endpoint.
- `GET api/lookups/location-types`: location types with `id`, `code`, `name`, and `description` (nullable).

Use the returned integer IDs as `countryId`, `provinceId`, and `locationTypeId` when submitting an address. Select a country before requesting its provinces. Example HTTP requests are in `InsurancePortal.LocationService.API.http`.

### Locations (authenticated)

All endpoints under `api/locations` require a bearer token (`[Authorize]`).

- `POST api/locations`: creates a location or returns a matching existing location. Sets `CreatedBy` and `UpdatedBy` to the caller's user ID and `CreatedAt`/`UpdatedAt` to now. The duplicate-return path does not modify the existing record. Handled by `CreateLocationCommandHandler`.
- `GET api/locations/{id}`: returns a location by ID, or 404. Handled by `GetLocationByIdQueryHandler`.
- `GET api/locations`: returns all locations. Handled by `GetAllLocationsQueryHandler`.
- `PUT api/locations/{id}`: updates a location, or 404. Sets `UpdatedAt` to now and `UpdatedBy` to the caller's user ID. `CreatedBy` is not changed. Handled by `UpdateLocationCommandHandler`.
- `DELETE api/locations/{id}`: deletes a location (204), or 404. This is a hard delete (`ILocationRepository.DeleteAsync`). The handler sets `UpdatedAt`/`UpdatedBy` on the entity before removal, but since the row is deleted those values are not persisted. Handled by `DeleteLocationCommandHandler`.

Note: Quote Service only uses POST and GET by ID.

### Authentication and User Identity

- `LocationsController` is decorated with `[Authorize]`. Bearer tokens are validated by `JwtBearerDefaults.AuthenticationScheme` with issuer, audience, lifetime, and signing key validation (`Jwt:Issuer`, `Jwt:Audience`, `Jwt:Secret`).
- The acting user ID is `User.FindFirstValue(ClaimTypes.NameIdentifier)`, exposed as `CurrentUserId` (`string?`). The token's `sub` claim maps to `NameIdentifier` by default, and is expected to hold the user GUID.
- `CurrentUserId` is passed to the Create, Update, and Delete commands and stored in `CreatedBy`/`UpdatedBy`.

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
│   ├── DTOs/              (LocationRequest, LocationResponse, lookup responses)
│   ├── Mappings/          (LocationMapper)
│   ├── Queries/           (location and lookup queries and their handlers)
│   └── Repositories/      (ILocationRepository, ILookupRepository)
├── InsurancePortal.LocationService.Infrastructure/
│   ├── Data/              (LocationDbContext, Configurations/)
│   ├── Migrations/        (InitialCreate)
│   └── Repositories/      (LocationRepository, LookupRepository)
└── InsurancePortal.LocationService.API/
    └── Controllers/       (LocationsController, LookupsController)
```

Template files `WeatherForecastController.cs`, `WeatherForecast.cs`, and `Infrastructure/Class1.cs` have been removed.

API project files: `Program.cs` (startup and DI), `Controllers/LocationsController.cs`, `Controllers/LookupsController.cs`, `Properties/launchSettings.json`, `appsettings.json`, `appsettings.Development.json`, `InsurancePortal.LocationService.API.http`.

The Application project file declares a `Services` folder (`Folder Include`); no service classes are implemented.

---

## Configuration

- `ConnectionStrings:DefaultConnection`: SQL Server connection. Development value: `Server=(localdb)\\mssqllocaldb;Database=LocationService;Integrated Security=true;TrustServerCertificate=true;`. The database is created and migrated automatically at startup via `Database.Migrate()`.
- Logging: Default `Information`, `Microsoft.AspNetCore` `Warning`. `AllowedHosts`: `*`.
- In the Development environment: `UseSwagger()`, `UseSwaggerUI()` (`/swagger`), and `MapOpenApi()` (`/openapi/v1.json`) are enabled. Swashbuckle packages supply Swagger generation and UI; `Microsoft.AspNetCore.OpenApi` supplies the built-in OpenAPI document.
- Middleware pipeline mirrors Quote Service: Swagger (Development only), CORS (`AllowFrontend` policy), HTTPS redirection, authentication, authorization, controllers.
- `Jwt:Issuer` and `Jwt:Audience`: both `InsurancePortal.IdentityService.Api`. `Jwt:Secret`: HMAC signing key (`SymmetricSecurityKey`, UTF-8 bytes). Issuer, audience, lifetime, and issuer signing key are validated. The secret is currently a development placeholder.
- Launch profiles: `http` (`http://localhost:5159`, `ASPNETCORE_ENVIRONMENT=Development`, `dotnetRunMessages`) and `IIS Express` (`http://localhost:50063/`, SSL port 44370, anonymous authentication enabled, Windows authentication disabled).
- Quote Service reads the URL from `LocationService:BaseUrl`.
- Target framework: `net10.0` with `Nullable` and `ImplicitUsings` enabled in all projects.
- Quote Service reads this URL from `LocationService:BaseUrl`.

---

## NuGet Packages

All projects target `net10.0`.

- **API**: `Microsoft.AspNetCore.Authentication.JwtBearer` 10.0.13, `Microsoft.AspNetCore.OpenApi` 10.0.12, `Microsoft.EntityFrameworkCore.Design` 10.0.13 (private assets), `Microsoft.EntityFrameworkCore.SqlServer` 10.0.13, `Swashbuckle.AspNetCore.Swagger` 10.3.0, `Swashbuckle.AspNetCore.SwaggerGen` 10.3.0, `Swashbuckle.AspNetCore.SwaggerUI` 10.3.0.
- **Infrastructure**: `Microsoft.EntityFrameworkCore` 10.0.13, `Microsoft.EntityFrameworkCore.Design` 10.0.13 (private assets), `Microsoft.EntityFrameworkCore.SqlServer` 10.0.13, `System.Security.Cryptography.Pkcs` 10.0.13.
- **Domain** and **Application**: no package references.

Project references: API -> Application, Infrastructure. Infrastructure -> Domain, Application. Application -> Domain.

---

## Open Items

- Replace the placeholder JWT signing key with a managed secret.
- Hard delete discards the audit stamp. Consider a soft delete (`IsDeleted`, `DeletedBy`, `DeletedAt`) if deletions must be auditable.
- Confirm the JWT settings match Identity Service.
