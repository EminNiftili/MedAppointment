# SQL & Migration Bələdçisi (PMC)

Bu sənəd EF Core migration əmrlərini **Package Manager Console (PMC)** üçün `--project` və `--startup` arqumentləri ilə izah edir.

## Əsas anlayışlar

- **`--project`**: Migration-ların saxlanılacağı layihə. Bu həll üçün adətən `MedAppointment.DataAccess` olur.
- **`--startup`**: `DbContext` üçün runtime konfiqurasiyanı (connection string və s.) yükləyən startup layihə. Bu, adətən API/Host layihəsidir.

> Qeyd: Hazırkı həll nümunəsində startup layihə ayrıca göstərilməyə bilər. Praktikada `DbContext`-in istifadə olunduğu host layihəni (`API`, `Web`, `Worker` və s.) `--startup` kimi seçin.

## Əmrlər (PMC)

Aşağıdakı nümunələrdə `--project` və `--startup` arqumentləri şərh olunub.

### 1) Migration yaratmaq

```powershell
Add-Migration InitialCreate --project MedAppointment.DataAccess --startup MedAppointment.Api
```

- `InitialCreate` — migration adı.
- `--project MedAppointment.DataAccess` — migration faylları bu layihədə saxlanır.
- `--startup MedAppointment.Api` — connection string və `DbContext` konfiqurasiyası buradan yüklənir.

### 2) DB-ni migration-larla yeniləmək

```powershell
Update-Database --project MedAppointment.DataAccess --startup MedAppointment.Api
```

Bu əmrlə ən son migration-lar DB-yə tətbiq edilir.

### 3) Migration silmək (son migration)

```powershell
Remove-Migration --project MedAppointment.DataAccess --startup MedAppointment.Api
```

Ən son migration geri alınır (DB-yə tətbiq edilibsə, əvvəlcə `Update-Database` ilə geri qaytarın).

### 4) Migration siyahısı

```powershell
Get-Migrations --project MedAppointment.DataAccess --startup MedAppointment.Api
```

Əlavə olunmuş bütün migration-ları göstərir.

### 5) Script çıxarmaq (SQL)

```powershell
Script-Migration --project MedAppointment.DataAccess --startup MedAppointment.Api -Output .\migrations.sql
```

Migration-ları SQL script kimi ixrac edir.

## Əlavə tövsiyələr

- Migration yaradanda konfiqurasiyaların (`Configurations`) düzgün tətbiq olunduğuna əmin olun.
- `DbContext`-ə bağlı connection string `appsettings.json`-da saxlanılmalıdır.
- `--startup` layihəsində `AddDbContext`/DI qeydiyyatının işlək olmasına diqqət edin.
