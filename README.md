# MangaTracker

## Requisitos

- .NET 10
- SQL Server LocalDB

## Configuración

Configurar los siguientes User Secrets:

ConnectionStrings:MangaTrackerConnectionString
Jwt:Key

## Ejecutar migraciones

dotnet ef database update --project MangaTracker.Infrastructure --startup-project MangaTracker.Api

## Ejecutar la API

dotnet run --project MangaTracker.Api