# DataAccess.Repositories

Bu qovluq DataAccess qatında repository interfeyslərini saxlayır. Repository-lər `IGenericRepository<TEntity>` üzərindən CRUD əməliyyatlarını təqdim edir və EF implementasiyaları `Implementations/EntityFramework/Repositories` altında yerləşir.

## Repository-lər və entity uyğunluğu

| Modul | Repository interfeysi | Entity |
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

## Əlaqəli qatlar (Referanslar)

| Qat | İstifadə məqsədi |
| --- | --- |
| `MedAppointment.DataAccess.UnitOfWorks` | Repository-ləri qruplaşdıran unit of work interfeysləri. |
| `MedAppointment.DataAccess.Implementations.EntityFramework.Repositories` | Repository interfeyslərinin EF Core implementasiyaları. |
| `MedAppointment.Entities` | Repository-lərin işlədiyi entity modelləri. |
