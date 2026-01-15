# SQL & Migration Guide (PMC)

This guide documents EF Core migration commands for **Package Manager Console (PMC)** using `--project` and `--startup` arguments.

## Core concepts

- **`--project`**: The project that stores the migrations. For this solution, it is typically `MedAppointment.DataAccess`.
- **`--startup`**: The startup project that loads runtime configuration (connection string, etc.). This is usually your API/Host project.

> Note: In this solution, the startup project may not be shown in the repository. In practice, use the host project (`API`, `Web`, `Worker`, etc.) that configures the `DbContext`.

## Commands (PMC)

The examples below explain how `--project` and `--startup` are used.

### 1) Create a migration

```powershell
Add-Migration InitialCreate -OutputDir Implementations/EntityFramework/SqlServer/Migrations --project MedAppointment.DataAccess --startup MedAppointment.Api
```

- `InitialCreate` — migration name.
- `--project MedAppointment.DataAccess` — migration files live here.
- `--startup MedAppointment.Api` — connection string and `DbContext` config are loaded here.

### 2) Apply migrations to the database

```powershell
Update-Database --project MedAppointment.DataAccess --startup MedAppointment.Api
```

Applies the latest migrations to the database.

### 3) Remove the last migration

```powershell
Remove-Migration --project MedAppointment.DataAccess --startup MedAppointment.Api
```

Removes the last migration (if applied to the DB, roll back first with `Update-Database`).

### 4) List migrations

```powershell
Get-Migrations --project MedAppointment.DataAccess --startup MedAppointment.Api
```

Lists all added migrations.

### 5) Generate a SQL script

```powershell
Script-Migration --project MedAppointment.DataAccess --startup MedAppointment.Api -Output .\migrations.sql
```

Exports migrations as a SQL script.

## Additional notes

- Ensure configurations under `Configurations` are applied correctly when generating migrations.
- The connection string should be set in `appsettings.json`.
- Make sure `AddDbContext`/DI registration is configured in the startup project.
