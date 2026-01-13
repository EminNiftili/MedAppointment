# Entities Layer

Bu qat domen obyektlərini (entity) saxlayır. Aşağıdakı cədvəllərdə mövcud entity-lər, onların aid olduğu modul və əsas əlaqələr göstərilir.

## Entity-lər üzrə baxış

| Modul | Entity | Qısa təsvir |
| --- | --- | --- |
| Classifier | `CurrencyEntity` | Valyuta siyahısı. |
| Classifier | `PaymentTypeEntity` | Ödəniş növü məlumatları. |
| Classifier | `PeriodEntity` | Görüş aralığı (dəqiqə ilə). |
| Classifier | `SpecialtyEntity` | Həkim ixtisasları. |
| Client | `AdminEntity` | Admin məlumatları. |
| Client | `DoctorEntity` | Həkim məlumatları. |
| Client | `OrganizationEntity` | Tibb müəssisəsi məlumatları. |
| Client | `PersonEntity` | Şəxsi məlumatlar. |
| Client | `UserEntity` | İstifadəçi əsas məlumatları. |
| Communication | `ChatEntity` | Çat mesajı. |
| Communication | `ChatHistoryEntity` | Çat tarixçəsi. |
| Communication | `MeetEntity` | Görüş/söhbət sessiyası. |
| Composition | `OrganizationUserEntity` | İstifadəçinin təşkilata bağlılığı. |
| Composition | `DoctorSpecialtyEntity` | Həkim-ixtisas əlaqəsi. |
| File | `ImageEntity` | Fayl/şəkil metadatası. |
| Payment | `PaymentEntity` | Ödəniş məlumatları. |
| Security | `DeviceEntity` | Cihaz məlumatları. |
| Security | `SessionEntity` | Sessiya məlumatları. |
| Security | `TokenEntity` | Token məlumatları. |
| Security | `TraditionalUserEntity` | Ənənəvi login məlumatları. |
| Service | `AppointmentEntity` | Qəbul (appointment). |
| Service | `DayPlanEntity` | Həftəlik plan (gün üzrə). |
| Service | `PeriodPlanEntity` | Müəyyən gün üçün period planı. |

## Enum kimi istifadə olunan property-lər

Aşağıdakı property-lər `byte`/`int` olaraq saxlanılır və kodda enum məntiqi ilə istifadə olunur.

| Entity | Property | Dəyərlər | İzah |
| --- | --- | --- | --- |
| `UserEntity` | `Provider` | `0` = Traditional, `1` = Google, `2` = Facebook, `3` = Apple | İstifadəçi qeydiyyat provayderi. |
| `PaymentEntity` | `Status` | `0` = Canceled, `1` = Pending, `2` = Partially Paid, `3` = Paid, `4` = Refund | Ödəniş statusu. |
| `DeviceEntity` | `DeviceType` | `0` = Android, `1` = iOS, `2` = Windows, `3` = Mac, `4` = Linux | Cihazın əməliyyat sistemi. |
| `DeviceEntity` | `AppType` | `0` = Web, `1` = Mobile | Cihazın tətbiq tipi. |
| `AppointmentEntity` | `SelectedServiceType` | `0` = OnSite, `1` = Online | Qəbulun xidmət tipi. |
| `DayPlanEntity` | `DayOfWeek` | `1` = Monday, `2` = Tuesday, `3` = Wednesday, `4` = Thursday, `5` = Friday, `6` = Saturday, `7` = Sunday | Həftənin günü. |

## Əsas referanslar (FK)

| Entity | Foreign Key | İstinad edilən entity | Naviqasiya property-si |
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
