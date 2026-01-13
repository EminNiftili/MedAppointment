# Entities Layer

This layer contains the domain entities. The tables below list the entities, their modules, and key relationships.

## Entity overview

| Module | Entity | Description |
| --- | --- | --- |
| Classifier | `CurrencyEntity` | Currency list. |
| Classifier | `PaymentTypeEntity` | Payment type data. |
| Classifier | `PeriodEntity` | Appointment interval (minutes). |
| Classifier | `SpecialtyEntity` | Doctor specialties. |
| Client | `AdminEntity` | Admin data. |
| Client | `DoctorEntity` | Doctor data. |
| Client | `OrganizationEntity` | Medical organization data. |
| Client | `PersonEntity` | Personal data. |
| Client | `UserEntity` | User core data. |
| Communication | `ChatEntity` | Chat message. |
| Communication | `ChatHistoryEntity` | Chat history. |
| Communication | `MeetEntity` | Meeting/session record. |
| Composition | `OrganizationUserEntity` | Organization-user relation. |
| Composition | `DoctorSpecialtyEntity` | Doctor-specialty relation. |
| File | `ImageEntity` | File/image metadata. |
| Payment | `PaymentEntity` | Payment data. |
| Security | `DeviceEntity` | Device data. |
| Security | `SessionEntity` | Session data. |
| Security | `TokenEntity` | Token data. |
| Security | `TraditionalUserEntity` | Traditional login data. |
| Service | `AppointmentEntity` | Appointment. |
| Service | `DayPlanEntity` | Weekly day plan. |
| Service | `PeriodPlanEntity` | Period plan for a specific day. |

## Enum-like properties

The following properties are stored as `byte`/`int` and are used as enums in code.

| Entity | Property | Values | Description |
| --- | --- | --- | --- |
| `UserEntity` | `Provider` | `0` = Traditional, `1` = Google, `2` = Facebook, `3` = Apple | User registration provider. |
| `PaymentEntity` | `Status` | `0` = Canceled, `1` = Pending, `2` = Partially Paid, `3` = Paid, `4` = Refund | Payment status. |
| `DeviceEntity` | `DeviceType` | `0` = Android, `1` = iOS, `2` = Windows, `3` = Mac, `4` = Linux | Device OS type. |
| `DeviceEntity` | `AppType` | `0` = Web, `1` = Mobile | Application type. |
| `AppointmentEntity` | `SelectedServiceType` | `0` = OnSite, `1` = Online | Appointment service type. |
| `DayPlanEntity` | `DayOfWeek` | `1` = Monday, `2` = Tuesday, `3` = Wednesday, `4` = Thursday, `5` = Friday, `6` = Saturday, `7` = Sunday | Day of week. |

## Key references (FK)

| Entity | Foreign Key | Referenced entity | Navigation property |
| --- | --- | --- | --- |
| `UserEntity` | `PersonId` | `PersonEntity` | `Person` |
| `PaymentEntity` | `PaymentTypeId` | `PaymentTypeEntity` | `PaymentType` |
| `AppointmentEntity` | `PaymentId` | `PaymentEntity` | `Payment` |
| `AppointmentEntity` | `PeriodPlanId` | `PeriodPlanEntity` | `PeriodPlan` |
| `DayPlanEntity` | `DoctorId` | `DoctorEntity` | `Doctor` |
| `DayPlanEntity` | `SpecialtyId` | `SpecialtyEntity` | `Specialty` |
| `DayPlanEntity` | `PeriodId` | `PeriodEntity` | `Period` |
| `OrganizationUserEntity` | `OrganizationId` | `OrganizationEntity` | `Organization` |
| `OrganizationUserEntity` | `UserId` | `UserEntity` | `User` |
| `DoctorSpecialtyEntity` | `DoctorId` | `DoctorEntity` | `Doctor` |
| `DoctorSpecialtyEntity` | `SpecialtyId` | `SpecialtyEntity` | `Specialty` |
