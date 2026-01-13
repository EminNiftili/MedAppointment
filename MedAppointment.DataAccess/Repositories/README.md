# DataAccess.Repositories

This folder contains DataAccess repository interfaces. Repositories provide CRUD operations via `IGenericRepository<TEntity>`, and EF implementations live under `Implementations/EntityFramework/Repositories`.

## Repository-to-entity map

| Module | Repository interface | Entity |
| --- | --- | --- |
| Classifier | `ICurrencyRepository` | `CurrencyEntity` |
| Classifier | `IPaymentTypeRepository` | `PaymentTypeEntity` |
| Classifier | `IPeriodRepository` | `PeriodEntity` |
| Classifier | `ISpecialtyRepository` | `SpecialtyEntity` |
| Client | `IAdminRepository` | `AdminEntity` |
| Client | `IDoctorRepository` | `DoctorEntity` |
| Client | `IOrganizationRepository` | `OrganizationEntity` |
| Client | `IPersonRepository` | `PersonEntity` |
| Client | `IUserRepository` | `UserEntity` |
| Communication | `IChatHistoryRepository` | `ChatHistoryEntity` |
| Communication | `IChatRepository` | `ChatEntity` |
| Communication | `IMeetRepository` | `MeetEntity` |
| Composition | `IOrganizationUserRepository` | `OrganizationUserEntity` |
| File | `IImageRepository` | `ImageEntity` |
| Payment | `IPaymentRepository` | `PaymentEntity` |
| Security | `IDeviceRepository` | `DeviceEntity` |
| Security | `ISessionRepository` | `SessionEntity` |
| Security | `ITraditionalUserRepository` | `TraditionalUserEntity` |
| Security | `ITokenRepository` | `TokenEntity` |
| Service | `IAppointmentRepository` | `AppointmentEntity` |
| Service | `IDayPlanRepository` | `DayPlanEntity` |
| Service | `IPeriodPlanRepository` | `PeriodPlanEntity` |

## Related layers (References)

| Layer | Purpose |
| --- | --- |
| `MedAppointment.DataAccess.UnitOfWorks` | Unit-of-work interfaces that group repositories. |
| `MedAppointment.DataAccess.Implementations.EntityFramework.Repositories` | EF Core implementations of repository interfaces. |
| `MedAppointment.Entities` | Entity models used by repositories. |
