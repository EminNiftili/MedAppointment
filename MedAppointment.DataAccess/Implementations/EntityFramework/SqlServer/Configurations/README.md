# EF Core Configurations (SqlServer)

Bu qovluq EF Core model konfigurasiya siniflərini saxlayır. Hər konfigurasiya `IEntityTypeConfiguration<TEntity>` üzərindən entity-lərin xəritələnməsini (mappings) müəyyən edir.

## Konfiqurasiya xəritəsi

| Modul | Konfiqurasiya sinfi | Entity |
| --- | --- | --- |
| Base | `BaseConfig<TEntity>` | `BaseEntity` törəmələri üçün ümumi qaydalar |
| Classifier | `BaseClassfierConfig<TEntity>` | `BaseClassfierEntity` törəmələri |
| Classifier | `CurrencyConfig` | `CurrencyEntity` |
| Classifier | `PaymentTypeConfig` | `PaymentTypeEntity` |
| Classifier | `PeriodConfig` | `PeriodEntity` |
| Classifier | `SpecialtyConfig` | `SpecialtyEntity` |
| Client | `AdminConfig` | `AdminEntity` |
| Client | `DoctorConfig` | `DoctorEntity` |
| Client | `OrganizationConfig` | `OrganizationEntity` |
| Client | `PersonConfig` | `PersonEntity` |
| Client | `UserConfig` | `UserEntity` |
| Communication | `ChatConfig` | `ChatEntity` |
| Communication | `ChatHistoryConfig` | `ChatHistoryEntity` |
| Communication | `MeetConfig` | `MeetEntity` |
| Composition | `DoctorSpecialtyConfig` | `DoctorSpecialtyEntity` |
| Composition | `OrganizationUserConfig` | `OrganizationUserEntity` |
| File | `ImageConfig` | `ImageEntity` |
| Payment | `PaymentConfig` | `PaymentEntity` |
| Security | `DeviceConfig` | `DeviceEntity` |
| Security | `SessionConfig` | `SessionEntity` |
| Security | `TokenConfig` | `TokenEntity` |
| Security | `TraditionalUserConfig` | `TraditionalUserEntity` |
| Service | `AppointmentConfig` | `AppointmentEntity` |
| Service | `DayPlanConfig` | `DayPlanEntity` |
| Service | `PeriodPlanConfig` | `PeriodPlanEntity` |

## Əlaqəli qatlar (Referanslar)

| Qat | İstifadə məqsədi |
| --- | --- |
| `MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Contexts` | Konfiqurasiyalar `DbContext` daxilində tətbiq edilir. |
| `MedAppointment.Entities` | Konfiqurasiyalar entity modellərinə aiddir. |
