# Dayaq Server – API & Services Report

Generated from the `dayaq-server` codebase. Base route for all APIs: **`/api/[controller]`**.

---

## 1. API Endpoints Summary

### 1.1 Auth & Users (anonymous or JWT)

| Method | Route | Controller | Auth | Description |
|--------|--------|------------|------|-------------|
| POST | `/api/Login` | LoginController | Anonymous | Traditional login (email/password). Returns access token; sets refresh token in cookie. |
| POST | `/api/Login/refresh` | LoginController | Anonymous | Refresh access token (body or cookie `RefreshToken`). |
| POST | `/api/Registration` | RegistrationController | Anonymous | Traditional user registration. |
| POST | `/api/Registration/Google` | RegistrationController | Anonymous | Placeholder – returns BadRequest. |
| POST | `/api/Registration/Facebook` | RegistrationController | Anonymous | Placeholder – returns BadRequest. |
| POST | `/api/Registration/Apple` | RegistrationController | Anonymous | Placeholder – returns BadRequest. |

### 1.2 Admin – Users (SystemAdmin only)

| Method | Route | Controller | Auth | Description |
|--------|--------|------------|------|-------------|
| GET | `/api/AdminUsers` | AdminUsersController | SystemAdmin | Paginated list of users. |
| GET | `/api/AdminUsers/{userId}` | AdminUsersController | SystemAdmin | User details by ID. |
| DELETE | `/api/AdminUsers/{userId}` | AdminUsersController | SystemAdmin | Remove user. |

### 1.3 Doctors (mixed: anonymous, Doctor, SystemAdmin)

| Method | Route | Controller | Auth | Description |
|--------|--------|------------|------|-------------|
| GET | `/api/Doctors` | DoctorsController | SystemAdmin | Paginated list of doctors. |
| GET | `/api/Doctors/{id}` | DoctorsController | SystemAdmin | Doctor by ID. |
| POST | `/api/Doctors/register` | DoctorsController | Anonymous | Register a doctor (traditional). |
| PUT | `/api/Doctors/confirm/{doctorId}` | DoctorsController | SystemAdmin | Confirm doctor. |
| PUT | `/api/Doctors/confirmSpecialty/{doctorId}/{specialtyId}` | DoctorsController | SystemAdmin | Confirm doctor specialty. |
| POST | `/api/Doctors/{doctorId}/specialties` | DoctorsController | SystemAdmin | Add specialty to doctor. |
| DELETE | `/api/Doctors/{doctorId}/specialties/{specialtyId}` | DoctorsController | Doctor, SystemAdmin | Remove doctor specialty. |
| POST | `/api/Doctors/schemas` | DoctorsController | Doctor | Add doctor schedule schema. |
| POST | `/api/Doctors/calendar/fill-from-weekly-schema` | DoctorsController | Doctor | Create day plans from weekly schema. |

### 1.4 Classifiers (CRUD – read often anonymous; create/update SystemAdmin)

Each of the following controllers follows the same pattern. **Read**: GET list (with query/pagination), GET by id. **Write**: POST (create), PUT `{id}` (update). Only **Currencies** is listed in full; the others share the same shape.

| Method | Route | Auth | Notes |
|--------|--------|------|--------|
| GET | `/api/Currencies` | Anonymous | List with pagination. |
| GET | `/api/Currencies/{id}` | Anonymous | By ID. |
| POST | `/api/Currencies` | SystemAdmin | Create. |
| PUT | `/api/Currencies/{id}` | SystemAdmin | Update. |

**Same pattern for:**

- **Languages** – `/api/Languages`, `/api/Languages/{id}`
- **PaymentTypes** – `/api/PaymentTypes`, `/api/PaymentTypes/{id}`
- **Periods** – `/api/Periods`, `/api/Periods/{id}`
- **Specialties** – `/api/Specialties`, `/api/Specialties/{id}`
- **PlanPaddingTypes** – `/api/PlanPaddingTypes`, `/api/PlanPaddingTypes/{id}`

---

## 2. Application Roles

- **SystemAdmin** – Admin panel: users, doctors, classifiers CRUD, confirm doctors/specialties.
- **Doctor** – Own schedules/schemas, calendar fill, manage own specialties (delete).
- **Anonymous** – Login, registration, doctor registration, classifier reads, refresh token.

---

## 3. Logic Services (MedAppointment.Logic)

Registered in `MedAppointment.Logic` → `DependencyInjectionExtension.AddLogicServices`.

### 3.1 Client & registration

| Interface | Implementation | Purpose |
|-----------|----------------|---------|
| IClientRegistrationService | ClientRegistrationService | User registration. |
| IPrivateClientInfoService | PrivateClientInfoService | Private client info. |
| IAdminUserService | AdminUserService | Admin user list/details/remove. |
| IDoctorService | DoctorService | Doctor CRUD, register, confirm, specialties. |

### 3.2 Plans & scheduling

| Interface | Implementation | Purpose |
|-----------|----------------|---------|
| IDoctorPlanManagerService | DoctorPlanManagerService | Doctor schemas, day plans from weekly schema. |
| ITimeSlotService | TimeSlotService | Time slot logic. |
| ITimeSlotPaddingStrategyResolver | TimeSlotPaddingStrategyResolver | Resolves padding strategy (NoPadding, StartOfPeriod, EndOfPeriod, LinearBetween, CenterBetween). |

### 3.3 Classifiers

| Interface | Implementation |
|-----------|----------------|
| ICurrencyService | CurrencyService |
| ILanguageService | LanguageService |
| IPaymentTypeService | PaymentTypeService |
| IPeriodService | PeriodService |
| ISpecialtyService | SpecialtyService |
| IPlanPaddingTypeService | PlanPaddingTypeService |

### 3.4 Localization

| Interface | Implementation |
|-----------|----------------|
| ILocalizerService | LocalizerService |
| ITranslationLookupService | TranslationLookupService |

### 3.5 Security (auth, tokens, hash)

| Interface | Implementation |
|-----------|----------------|
| IHashService | HashService |
| ILoginService | LoginService |
| ITokenService | JwtBearerTokenService |

---

## 4. Data Access – Repositories (MedAppointment.DataAccess)

All repositories are scoped. Grouped by unit of work.

### 4.1 Client & users

- IAdminRepository, IDoctorRepository, IOrganizationRepository, IPersonRepository, IUserRepository  
- **Unit:** IUnitOfClient

### 4.2 Classifiers

- ICurrencyRepository, ILanguageRepository, IPaymentTypeRepository, IPeriodRepository, ISpecialtyRepository, IPlanPaddingTypeRepository  
- **Unit:** IUnitOfClassifier

### 4.3 Communication

- IChatHistoryRepository, IChatRepository, IMeetRepository  
- **Unit:** IUnitOfCommunication

### 4.4 Doctor / plans

- IDayBreakRepository, IDaySchemaRepository, IWeeklySchemaRepository  
- **Unit:** IUnitOfDoctor

### 4.5 File

- IImageRepository  
- **Unit:** IUnitOfFile

### 4.6 Payment

- IPaymentRepository  
- **Unit:** IUnitOfPayment

### 4.7 Security

- IDeviceRepository, ISessionRepository, ITraditionalUserRepository, ITokenRepository  
- **Unit:** IUnitOfSecurity

### 4.8 Service (appointments & plans)

- IAppointmentRepository, IDayPlanRepository, IPeriodPlanRepository  
- **Unit:** IUnitOfService

### 4.9 Localization

- IResourceRepository, ITranslationRepository  
- **Unit:** IUnitOfLocalization

### 4.10 Composition

- IOrganizationUserRepository (used across areas)

---

## 5. Project Structure (high level)

```
MedAppointment.Api          – Web API, controllers, JWT, Swagger
MedAppointment.Logic        – Services, validation, AutoMapper
MedAppointment.DataAccess   – EF Core, DbContext, repositories, units of work
MedAppointment.DataTransferObjects – DTOs
MedAppointment.Entities     – Domain entities
MedAppointment.Validations  – FluentValidation
```

---

## 6. Not Exposed as HTTP (no controller)

- **Payments** – Repository and unit (IUnitOfPayment, IPaymentRepository) exist; no payment controller found.
- **Appointments** – Repositories (IAppointmentRepository, IDayPlanRepository, IPeriodPlanRepository) and IDoctorPlanManagerService exist; no dedicated appointment listing/booking controller.
- **Chat/Meet** – Communication repositories exist; no chat/meet controller.
- **Images** – IImageRepository exists; no image upload controller in this scan.
- **Sessions/Devices** – Used internally for security; no public API listed.

---

*Report generated from dayaq-server. For live routes and schemas, use Swagger UI (e.g. `/swagger`) when the API is running.*
