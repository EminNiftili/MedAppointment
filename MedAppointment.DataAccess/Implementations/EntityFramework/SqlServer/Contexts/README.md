# EF Core Contexts (SqlServer)

Bu qovluq EF Core `DbContext` və ona bağlı DbSet siyahılarını saxlayır. `MedicalAppointmentContext` tətbiqin bütün entity-lərini bir kontekstdə birləşdirir.

## Contextlər

| Context | Təyinat |
| --- | --- |
| `MedicalAppointmentContext` | SQL Server üzərində əsas tətbiq konteksti. |

## DbSet siyahısı

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

## Əlaqəli qatlar (Referanslar)

| Qat | İstifadə məqsədi |
| --- | --- |
| `MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations` | Entity mapping konfiqurasiyaları. |
| `MedAppointment.Entities` | DbSet-lərin saxladığı entity modelləri. |
