# Running the backend on Mac and testing with Swagger

The backend is developed on Windows with SQL Server. To run on Mac you need SQL Server (via Docker), the right config, and optional seed data. Follow these steps.

---

## 1. Prerequisites

- **.NET 9 SDK**  
  Install from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download) or:  
  `brew install dotnet@9`

- **Docker Desktop for Mac**  
  Install from [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop). Required to run SQL Server locally.

- **Git**  
  Clone the repo as usual.

---

## 2. SQL Server in Docker (Mac)

The project uses **SQL Server** and **Entity Framework Core** with SQL Server–specific migrations. On Mac, run SQL Server in Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 --name dayaq-sql \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

- **Password**: Must meet SQL Server policy (e.g. `YourStrong@Passw0rd`). Change it if you want, and use the same value in the connection string.
- **Port**: `1433` is the default. If it’s in use, change the host port (e.g. `1533:1433`) and use that port in the connection string.

Check that the container is running:

```bash
docker ps
```

---

## 3. Configuration for Mac

The repo’s `appsettings.json` has Windows-specific values:

- **ConnectionStrings:MedicalAppointmentContext** – Windows SQL Server instance.
- **Settings:FileServerPath** – e.g. `C:\MedAppointmentApp`.

Override these in Development so the app runs on Mac without changing committed files.

### Option A: `appsettings.Development.json` (recommended)

1. Copy the example file into **MedAppointment.Api** (so your local overrides stay out of git if you add `appsettings.Development.json` to `.gitignore`):

   ```bash
   cd MedAppointment.Api
   cp appsettings.Development.example.json appsettings.Development.json
   ```

2. Edit **appsettings.Development.json** if needed:
   - **ConnectionStrings:MedicalAppointmentContext**  
     Use the Docker connection string (see example below).  
     If you changed the SA password or host port, update it here.
   - **Settings:FileServerPath**  
     Use a path that exists on your Mac (e.g. `./uploads` or `/tmp/MedAppointmentApp`).

Example connection string for the Docker command above:

```json
"ConnectionStrings": {
  "MedicalAppointmentContext": "Server=localhost,1433;Database=DayaqDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
}
```

If you used a different host port (e.g. `1533:1433`):

```json
"Server=localhost,1533;..."
```

**Important:** Add `appsettings.Development.json` to `.gitignore` if it contains your real password, or keep it as a local-only override. The repo already has `appsettings.Development.example.json` as a template.

### Option B: Environment variables

You can override without creating the file:

```bash
export ConnectionStrings__MedicalAppointmentContext="Server=localhost,1433;Database=DayaqDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
export Settings__FileServerPath="./uploads"
```

Then run the API (see below). Same values work in your shell profile or in a small script.

---

## 4. Apply migrations and create the database

From the **solution root** (where the `.sln` is), run:

```bash
dotnet ef database update --project MedAppointment.DataAccess --startup-project MedAppointment.Api
```

This creates/updates the **DayaqDB** database in the SQL Server instance (Docker). If the connection string points to another server or port, that’s where the DB will be created.

---

## 5. Run the API and open Swagger

1. Set the environment to Development (so Swagger is enabled):

   ```bash
   export ASPNETCORE_ENVIRONMENT=Development
   ```

   Or on Windows (PowerShell): `$env:ASPNETCORE_ENVIRONMENT="Development"`

2. Run the API:

   ```bash
   cd MedAppointment.Api
   dotnet run
   ```

3. Open Swagger in the browser:

   - **HTTPS**: `https://localhost:7xxx/swagger` (check the console for the port).
   - **HTTP**: `http://localhost:5xxx/swagger` if applicable.

Swagger is only enabled when `ASPNETCORE_ENVIRONMENT=Development` (see **Program.cs**).

---

## 6. Mock / seed data

There is no automatic seed in the codebase. You can add data in two ways.

### A. Seed data in the repository (optional)

1. **Classifier data (currencies, periods, etc.)**  
   You can add a SQL script under e.g. **MedAppointment.Api/SQL/** or **MedAppointment.DataAccess/SQL/** that inserts rows into the classifier tables (after migrations have created them). Run it once against your local DB (e.g. with `sqlcmd` or Azure Data Studio) so the admin panel and registration flows have something to show.

2. **Test admin user**  
   - Use Swagger: **POST /api/Registration** with a body for traditional registration (name, email, password, etc.).
   - Then mark that user as admin in the database:
     - Find the new user’s `User.Id` (e.g. from the `Users` and `People` tables).
     - Insert into the `Admins` table: `UserId = <that id>`.
     - Insert into the `UserTypes` table (or equivalent) so the user has the SystemAdmin role (value `3`).
   - After that, you can use **POST /api/Login** with the same email/password and get a token with admin role, and test admin-only endpoints in Swagger (using the Bearer token).

### B. Use Swagger only (no SQL script)

- Call **POST /api/Registration** to create a normal user, then **POST /api/Login** to get a JWT.
- Use the token in Swagger (Authorize → Bearer &lt;token&gt;) to call endpoints that require auth.
- For admin-only endpoints you still need to promote a user to admin in the DB as above, or add a small seed/tool that creates one admin user.

---

## 7. Quick checklist

| Step | Action |
|------|--------|
| 1 | Install .NET 9 SDK and Docker Desktop on Mac |
| 2 | Start SQL Server: `docker run ...` (see section 2) |
| 3 | Copy `appsettings.Development.example.json` → `appsettings.Development.json` and set connection string + file path |
| 4 | `export ASPNETCORE_ENVIRONMENT=Development` |
| 5 | `dotnet ef database update --project MedAppointment.DataAccess --startup-project MedAppointment.Api` |
| 6 | `cd MedAppointment.Api && dotnet run` |
| 7 | Open `https://localhost:7xxx/swagger` (or the port shown) |
| 8 | (Optional) Run a SQL seed script for classifier data and/or create an admin user as in section 6 |

---

## 8. Troubleshooting

- **Connection refused / timeout**  
  Ensure the SQL Server container is running (`docker ps`) and the port in the connection string matches the one mapped on the host (e.g. `1433` or `1533`).

- **Swagger not available**  
  Swagger is only registered when `ASPNETCORE_ENVIRONMENT=Development`. Set that and restart the API.

- **Migrations fail**  
  Run from the solution root and use `--project MedAppointment.DataAccess --startup-project MedAppointment.Api` so the startup project’s config (including connection string) is used.

- **File upload / file server errors**  
  Ensure `Settings:FileServerPath` in Development points to a directory that exists and is writable on your Mac (e.g. `./uploads`).

- **Password / login**  
  If you seed an admin via SQL, the password must be stored as the same hash the app uses (HashService: SHA256(password + email) then Base64). Easiest is to create the user via **POST /api/Registration** and then only add the Admin row and UserType in the DB.
