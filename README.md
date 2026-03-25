# NotesMicroservice

`NotesMicroservice` is a small ASP.NET Core service for managing notes. It exposes a simple HTTP API for creating, reading, updating, listing, and deleting notes, and currently supports both SQL Server and SQLite storage through the same repository abstraction.

The current development setup is oriented around SQLite so the service can be run locally without external infrastructure.

## Features

- Create notes
- Get a note by id
- Get paged notes
- Update notes with optimistic concurrency via `NoteVersion`
- Soft-delete notes via `DeletedOn`
- Switch database provider via configuration

## Project Structure

- [`src/Application`](/home/deck/Documents/GitHub/NotesMicroservice/src/Application): API, MediatR requests/handlers, validators, startup/config
- [`src/Domain`](/home/deck/Documents/GitHub/NotesMicroservice/src/Domain): domain entities and interfaces
- [`src/Infrastructure`](/home/deck/Documents/GitHub/NotesMicroservice/src/Infrastructure): repository implementation, mappings, persistence models
- [`tests/Application.UnitTests`](/home/deck/Documents/GitHub/NotesMicroservice/tests/Application.UnitTests): unit tests

## Requirements

- .NET 8 SDK
- Rider, VS Code, or `dotnet` CLI

## Configuration

The app loads:

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. environment variables

Important settings:

- `Db:Provider`: `Sqlite` or `SqlServer`
- `ConnectionStrings:Default`: active database connection string

Development currently uses SQLite in [`src/Application/appsettings.Development.json`](/home/deck/Documents/GitHub/NotesMicroservice/src/Application/appsettings.Development.json).

## Local Development Setup

### 1. Use Development environment

Set these environment variables in your run configuration:

```text
DOTNET_ENVIRONMENT=Development
ASPNETCORE_ENVIRONMENT=Development
```

### 2. Create or connect to the SQLite database

The development connection string points to:

```text
/home/deck/db/notes.db
```

If you want to inspect it in Rider, use:

```text
jdbc:sqlite:/home/deck/db/notes.db
```

### 3. Apply the SQLite schema

Run the script in:

- [`src/Application/database/schema.sqlite.sql`](/home/deck/Documents/GitHub/NotesMicroservice/src/Application/database/schema.sqlite.sql)

This creates the `notes` table and index expected by the repository.

### 4. Run the application

From Rider:

- run the `Application` configuration with `Development` environment variables set

From CLI:

```bash
DOTNET_ENVIRONMENT=Development ASPNETCORE_ENVIRONMENT=Development dotnet run --project src/Application/Application.csproj
```

## Docker

Notes to self:

- [`Dockerfile`](/home/deck/Documents/GitHub/NotesMicroservice/Dockerfile)
- [`docker/entrypoint.sh`](/home/deck/Documents/GitHub/NotesMicroservice/docker/entrypoint.sh)
- image ownership now lives in this repo instead of the frontend repo
- entrypoint initializes SQLite schema automatically if the database file does not exist
- this is what the frontend `docker compose` setup now builds against

## API Overview

Base route:

```text
/api/Note
```

Endpoints:

- `POST /api/Note` creates a note
- `GET /api/Note/{id}` gets a note by id
- `GET /api/Note?page=1&pageSize=10` gets paged notes
- `PUT /api/Note` updates a note
- `DELETE /api/Note/{id}` soft-deletes a note

### Create request body

```json
{
  "title": "My first note",
  "text": "Example content"
}
```

### Update request body

```json
{
  "id": 1,
  "title": "Updated title",
  "text": "Updated content",
  "noteVersion": 1
}
```

## Postman

A Postman collection is included here:

- [`postman/NotesMicroservice.postman_collection.json`](/home/deck/Documents/GitHub/NotesMicroservice/postman/NotesMicroservice.postman_collection.json)

Import it and set `baseUrl` to wherever the app is running locally.

## Notes

- Create requests do not accept `id`; the database generates it.
- Update requests require `noteVersion` for optimistic concurrency.
- Deletes are soft deletes, not hard deletes.

## Todo

- Add integration API tests
- Add integration database tests
- Add automated database initialization/migrations on startup
