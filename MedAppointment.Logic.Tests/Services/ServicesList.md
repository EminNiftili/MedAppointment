# Logic Layer – Servislərin siyahısı (Unit test planı)

Unit testlər **tək-tək** yazılacaq. Hər servis üçün ayrıca test faylı (və lazım gələndə Magic data) əlavə edin.

---

## 1. Classifier servisləri

| # | İnterfeys | İmplementasiya | Test faylı (yazılacaq) | Status |
|---|-----------|----------------|-------------------------|--------|
| 1 | `ICurrencyService` | `CurrencyService` | `CurrencyServiceTests.cs` | ✅ |
| 2 | `ILanguageService` | `LanguageService` | `LanguageServiceTests.cs` | ✅ |
| 3 | `ISpecialtyService` | `SpecialtyService` | `SpecialtyServiceTests.cs` | ✅ |
| 4 | `IPeriodService` | `PeriodService` | `PeriodServiceTests.cs` | ✅ |
| 5 | `IPaymentTypeService` | `PaymentTypeService` | `PaymentTypeServiceTests.cs` | ✅ |
| 6 | `IPlanPaddingTypeService` | `PlanPaddingTypeService` | `PlanPaddingTypeServiceTests.cs` | ✅ |

---

## 2. Client servisləri

| # | İnterfeys | İmplementasiya | Test faylı (yazılacaq) | Status |
|---|-----------|----------------|-------------------------|--------|
| 7 | `IDoctorService` | `DoctorService` | `DoctorServiceTests.cs` | ✅ |
| 8 | `IAdminUserService` | `AdminUserService` | `AdminUserServiceTests.cs` | ✅ |
| 9 | `IPrivateClientInfoService` | `PrivateClientInfoService` | `PrivateClientInfoServiceTests.cs` | ✅ |
| 10 | `IClientRegistrationService` | `ClientRegistrationService` | `ClientRegistrationServiceTests.cs` | ✅ |

---

## 3. Security servisləri

| # | İnterfeys | İmplementasiya | Test faylı (yazılacaq) | Status |
|---|-----------|----------------|-------------------------|--------|
| 11 | `IHashService` | `HashService` | `HashServiceTests.cs` | ✅ |
| 12 | `ITokenService` | `JwtBearerTokenService` | `JwtBearerTokenServiceTests.cs` | ✅ |
| 13 | `ILoginService` | `LoginService` | `LoginServiceTests.cs` | ✅ |

---

## 4. Localization servisləri

| # | İnterfeys | İmplementasiya | Test faylı (yazılacaq) | Status |
|---|-----------|----------------|-------------------------|--------|
| 14 | `ILocalizerService` | `LocalizerService` | `LocalizerServiceTests.cs` | ✅ |
| 15 | `ITranslationLookupService` | `TranslationLookupService` | `TranslationLookupServiceTests.cs` | ✅ |

---

## 5. Calendar & Plan & Schedule servisləri

| # | İnterfeys | İmplementasiya | Test faylı (yazılacaq) | Status |
|---|-----------|----------------|-------------------------|--------|
| 16 | `IDoctorCalendarService` | `DoctorCalendarService` | `DoctorCalendarServiceTests.cs` | ✅ |
| 17 | `IDoctorPlanManagerService` | `DoctorPlanManagerService` | `DoctorPlanManagerServiceTests.cs` | ✅ |
| 18 | `ITimeSlotService` | `TimeSlotService` | `TimeSlotServiceTests.cs` | ✅ |

---

## Cəmi: 18 servis

İlk addım: **1 – CurrencyService** üçün testlər (Magic data artıq `Magic/` qovluğunda hazırdır).

Status: ⬜ yazılmayıb | ✅ yazılıb
