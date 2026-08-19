# MangaTracker

MangaTracker is a REST API developed with **.NET 10** that allows users to manage their manga collections and track the volumes they own.

The project has been developed as a personal project with a focus on **Clean Architecture, Domain-Driven Design (DDD), CQRS, authentication, testing and containerization**.

## Architecture

The solution follows a **Clean Architecture** approach, separating responsibilities between different layers:

```text
MangaTracker
├── MangaTracker.Api
├── MangaTracker.Application
├── MangaTracker.Domain
├── MangaTracker.Infrastructure
└── MangaTracker.Tests
```

### MangaTracker.Api

Entry point of the application.

Responsible for:

* REST API controllers.
* HTTP request/response handling.
* Dependency Injection configuration.
* Authentication and authorization configuration.
* Swagger/OpenAPI configuration.

### MangaTracker.Application

Contains the application's use cases and business workflows.

Responsible for:

* Commands and queries.
* Command/query handlers.
* DTOs.
* Validators.
* Application-level abstractions.

### MangaTracker.Domain

Contains the core domain model and business rules.

Responsible for:

* Domain entities.
* Domain relationships.
* Business concepts independent of infrastructure concerns.

### MangaTracker.Infrastructure

Contains implementations related to external concerns.

Responsible for:

* Entity Framework Core.
* SQL Server persistence.
* Repository implementations.
* Database configurations.
* EF Core migrations.
* JWT and security infrastructure.

### MangaTracker.Tests

Contains the unit tests for the application.

The project uses **xUnit** and mocks infrastructure dependencies where appropriate.

---

## Technologies

* **.NET 10**
* **ASP.NET Core Web API**
* **C#**
* **Entity Framework Core**
* **SQL Server**
* **Docker**
* **Docker Compose**
* **JWT Authentication**
* **Swagger / OpenAPI**
* **FluentValidation**
* **xUnit**
* **Clean Architecture**
* **Domain-Driven Design**
* **CQRS**

---

## Features

### Authentication

* User registration.
* User login.
* Password hashing.
* JWT-based authentication.
* Protected API endpoints.

### Manga management

* Manga entity management.
* Manga information including title, publisher, total volumes and cover URL.

### Collection management

Authenticated users can:

* Create a manga collection.
* Retrieve their collections.
* Delete a manga from their collection.
* Add volumes to a collection.
* Retrieve the volumes owned for a collection.
* Delete volumes from a collection.

---

## Requirements

To run the project using Docker:

* [Docker](https://www.docker.com/) with Docker Compose support.

For local development without Docker:

* **.NET 10 SDK**
* **SQL Server** or SQL Server LocalDB.

---

# Running with Docker

Docker Compose is the recommended way to run the complete application.

The Docker environment consists of two services:

```text
┌─────────────────────┐
│   MangaTracker API  │
│      :8080          │
└──────────┬──────────┘
           │
           │
┌──────────▼──────────┐
│      SQL Server     │
│       :1433         │
└─────────────────────┘
```

SQL Server uses a Docker volume to persist database data between container restarts.

The API waits for SQL Server to become healthy before starting.

## Environment variables

The project includes a `.env.example` file containing the environment variables required by Docker Compose.

The required variables are:

```text
MSSQL_SA_PASSWORD
JWT_KEY
JWT_ISSUER
JWT_AUDIENCE
```

Create a `.env` file based on `.env.example` and provide your own values.

**Never commit real passwords, JWT keys or other secrets to the repository.**

For local development, you can alternatively use **ASP.NET Core User Secrets**.

The following configuration keys are required when running the API locally:

```text
ConnectionStrings:MangaTrackerConnectionString
Jwt:Key
```

---

## Start the application

From the solution root:

```bash
docker compose up --build
```

This command builds the API image and starts both the API and SQL Server containers.

The API will be available at:

```text
http://localhost:8080
```

SQL Server will be available on:

```text
localhost:1433
```

## Stop the application

To stop the containers:

```bash
docker compose down
```

The SQL Server data is stored in the `sqlserver_data` Docker volume, so stopping the containers does not remove the database data.

To remove the containers and the associated volume:

```bash
docker compose down -v
```

> Removing the volume deletes the SQL Server database data.

---

# Database migrations

The project uses **Entity Framework Core migrations** to manage database schema changes.

When SQL Server is running, migrations can be applied using:

```bash
dotnet ef database update --project MangaTracker.Infrastructure --startup-project MangaTracker.Api
```

This command applies the pending migrations to the configured database.

---

# Running locally

The application can also be run without Docker.

Make sure a SQL Server instance is available and configure the required connection string using **User Secrets**.

Configure:

```text
ConnectionStrings:MangaTrackerConnectionString
Jwt:Key
```

Then apply the migrations:

```bash
dotnet ef database update --project MangaTracker.Infrastructure --startup-project MangaTracker.Api
```

Finally, start the API:

```bash
dotnet run --project MangaTracker.Api
```

---

# Swagger / OpenAPI

Swagger is available in the **Development** environment.

When running the application locally or through Docker in Development, Swagger can be accessed at:

```text
http://localhost:8080/swagger
```

Swagger provides an interactive interface for exploring and testing the API.

Swagger is intended as a development tool and is not exposed in production environments.

---

# Testing

The project uses **xUnit** for unit testing.

Tests can be executed with:

```bash
dotnet test
```

The test suite covers the main application use cases and business scenarios, including validation and collection management.

---

# Project structure

```text
MangaTracker
│
├── MangaTracker.Api
│   ├── Controllers
│   └── Models
│
├── MangaTracker.Application
│   ├── Contracts
│   ├── Errors
|   ├── Responses
│   └── Features
|       └── Feature XXX
|           ├── Commands
|           ├── Queries
│           ├── DTOs
│           └── Validators
│
├── MangaTracker.Domain
│   ├── Entities
│   └── Enums
│
├── MangaTracker.Infrastructure
│   ├── Persistence
│   ├── Security
│   └── Migrations
│    
│
└── MangaTracker.Tests
```

The dependency direction follows the principles of Clean Architecture, keeping the domain independent from infrastructure and external frameworks.

---

# Development goals

MangaTracker is an evolving personal project. The main goal is not only to build a functional manga collection API, but also to use the project to explore and demonstrate professional software development practices.

Current areas of focus include:

* Clean Architecture.
* Domain-Driven Design.
* CQRS.
* REST API design.
* Automated testing.
* Authentication and authorization.
* Entity Framework Core.
* SQL Server.
* Docker and containerized development.
* Configuration and secret management.

Future functionality can include manga release tracking, notifications, richer collection management and additional integrations.

---

## License

This project is a personal project created for learning and portfolio purposes.
