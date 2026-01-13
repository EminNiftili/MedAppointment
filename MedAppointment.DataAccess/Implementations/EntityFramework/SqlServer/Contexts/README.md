# EF Core Contexts (SqlServer)

This folder contains EF Core `DbContext` types and their DbSet declarations. `MedicalAppointmentContext` aggregates all entities in a single context.

## Contexts

| Context | Purpose |
| --- | --- |
| `MedicalAppointmentContext` | Primary SQL Server context for the application. |

## DbSet list

| DbSet | Entity |
| --- | --- |
| `Currencies` | `CurrencyEntity` |
| `PaymentTypes` | `PaymentTypeEntity` |
| `Periods` | `PeriodEntity` |
| `Specialties` | `SpecialtyEntity` |
| `Admins` | `AdminEntity` |
| `Doctors` | `DoctorEntity` |
| `People` | `PersonEntity` |
| `Users` | `UserEntity` |
| `Organizations` | `OrganizationEntity` |
| `ChatHistories` | `ChatHistoryEntity` |
| `Chats` | `ChatEntity` |
| `Meets` | `MeetEntity` |
| `OrganizationUsers` | `OrganizationUserEntity` |
| `DoctorSpecialties` | `DoctorSpecialtyEntity` |
| `Images` | `ImageEntity` |
| `Payments` | `PaymentEntity` |
| `Devices` | `DeviceEntity` |
| `Sessions` | `SessionEntity` |
| `Tokens` | `TokenEntity` |
| `TraditionalUsers` | `TraditionalUserEntity` |
| `Appointments` | `AppointmentEntity` |
| `DayPlans` | `DayPlanEntity` |
| `PeriodPlans` | `PeriodPlanEntity` |

## Related layers (References)

| Layer | Purpose |
| --- | --- |
| `MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations` | Entity mapping configurations. |
| `MedAppointment.Entities` | Entity models stored by DbSets. |
