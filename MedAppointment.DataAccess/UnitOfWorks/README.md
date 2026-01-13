# DataAccess.UnitOfWorks

This folder contains unit-of-work interfaces. Each unit of work groups related repositories and provides a single transaction boundary (`SaveChanges`).

## Unit of work map

| Unit of work interface | Repositories |
| --- | --- |
| `IUnitOfClassifier` | `ICurrencyRepository`, `IPaymentTypeRepository`, `IPeriodRepository`, `ISpecialtyRepository` |
| `IUnitOfClient` | `IAdminRepository`, `IDoctorRepository`, `IPersonRepository`, `IUserRepository` |
| `IUnitOfCommunication` | `IChatHistoryRepository`, `IChatRepository`, `IMeetRepository` |
| `IUnitOfFile` | `IImageRepository` |
| `IUnitOfPayment` | `IPaymentRepository` |
| `IUnitOfSecurity` | `IDeviceRepository`, `ISessionRepository`, `ITokenRepository`, `ITraditionalUserRepository` |
| `IUnitOfService` | `IAppointmentRepository`, `IDayPlanRepository`, `IPeriodPlanRepository` |

## Core contract

| Interface | Methods |
| --- | --- |
| `IUnitOfWork` | `SaveChanges()`, `SaveChangesAsync()` |

## Related layers (References)

| Layer | Purpose |
| --- | --- |
| `MedAppointment.DataAccess.Repositories` | Repository interfaces used by units of work. |
| `MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks` | EF Core implementations of unit-of-work interfaces. |
