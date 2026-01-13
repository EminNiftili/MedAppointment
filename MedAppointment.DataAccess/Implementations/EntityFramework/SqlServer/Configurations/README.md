# EF Core Configurations (SqlServer)

This folder holds EF Core model configuration classes. Each configuration maps entities via `IEntityTypeConfiguration<TEntity>`.

## Configuration map

| Module | Configuration class | Entity |
| --- | --- | --- |
| Base | `BaseConfig<TEntity>` | Common rules for `BaseEntity` descendants |
| Classifier | `BaseClassfierConfig<TEntity>` | Rules for `BaseClassfierEntity` descendants |
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

## Related layers (References)

| Layer | Purpose |
| --- | --- |
| `MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Contexts` | Configurations are applied inside the `DbContext`. |
| `MedAppointment.Entities` | Entity models that these configurations map. |
