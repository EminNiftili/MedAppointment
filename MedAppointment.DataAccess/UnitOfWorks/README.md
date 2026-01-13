# DataAccess.UnitOfWorks

Bu qovluq unit of work interfeyslərini saxlayır. Hər unit of work müəyyən domain repository-lərini birləşdirərək vahid tranzaksiya (SaveChanges) idarəçiliyi təmin edir.

## Unit of work xəritəsi

| Unit of work interfeysi | Repository-lər |
| --- | --- |
| `IUnitOfClassifier` | `ICurrencyRepository`, `IPaymentTypeRepository`, `IPeriodRepository`, `ISpecialtyRepository` |
| `IUnitOfClient` | `IAdminRepository`, `IDoctorRepository`, `IPersonRepository`, `IUserRepository` |
| `IUnitOfCommunication` | `IChatHistoryRepository`, `IChatRepository`, `IMeetRepository` |
| `IUnitOfFile` | `IImageRepository` |
| `IUnitOfPayment` | `IPaymentRepository` |
| `IUnitOfSecurity` | `IDeviceRepository`, `ISessionRepository`, `ITokenRepository`, `ITraditionalUserRepository` |
| `IUnitOfService` | `IAppointmentRepository`, `IDayPlanRepository`, `IPeriodPlanRepository` |

## Əsas contract

| İnterfeys | Metodlar |
| --- | --- |
| `IUnitOfWork` | `SaveChanges()`, `SaveChangesAsync()` |

## Əlaqəli qatlar (Referanslar)

| Qat | İstifadə məqsədi |
| --- | --- |
| `MedAppointment.DataAccess.Repositories` | Unit of work tərkibindəki repository interfeysləri. |
| `MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks` | Unit of work interfeyslərinin EF Core implementasiyaları. |
