# Magic – Test Data

Bu qovluq unit testlər üçün sabit (magic) test məlumatlarını saxlayır. Real DB-dən asılı olmadan servisləri test etmək üçün bu datalardan istifadə edin.

## Qaydalar

- **MagicIds** – Testlərdə istifadə olunan ID-lər (CurrencyIdOne, LanguageIdOne və s.) bir yerdə toplanır.
- **Magic{Entity}** – Hər entity/DTO üçün ayrı fayl (məs: `MagicCurrency.cs`, `MagicLanguage.cs`). Servis testləri yazıldıqca əlavə edin.
- Yeni magic data əlavə edərkən ardıcıllığa və oxunaqlığa əməl edin.

## Mövcud fayllar

| Fayl | Təsvir |
|------|--------|
| `MagicIds.cs` | Ümumi test ID-ləri |
| `MagicClassifierHelper.cs` | Classifier entity-lər üçün Resource/Translation helper |
| `MagicCurrency.cs` | Currency entity və DTO-lar üçün test dataları |
| `MagicLanguage.cs` | Language entity və DTO-lar üçün test dataları |
| `MagicSpecialty.cs` | Specialty entity və DTO-lar üçün test dataları |
| `MagicPeriod.cs` | Period entity və DTO-lar üçün test dataları |
| `MagicPaymentType.cs` | PaymentType entity və DTO-lar üçün test dataları |
| `MagicPlanPaddingType.cs` | PlanPaddingType entity və DTO-lar üçün test dataları |
| `MagicCalendar.cs` | Calendar/Plan: DoctorId, DayPlan, PeriodPlan, CreateDayPlansFromSchemaDto, DoctorSchemaCreateDto |
| `MagicClient.cs` | Client: User, Doctor, Person, OrganizationUser, pagination DTOs, TraditionalUserRegister, AdminDoctorSpecialtyCreate |
| `MagicSecurity.cs` | Security/Login: DeviceDto, TraditionalUserLoginDto, RefreshTokenRequestDto, PersonWithTraditionalUser |

Servis testləri hissə-hissə yazıldıqca bu cədvələ yeni Magic fayllar əlavə edin.
