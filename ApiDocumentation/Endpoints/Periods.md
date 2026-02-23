# Periods Controller

Bütün classifier endpointləri aşağıdakı ardıcıllıqla təsvir olunur: 1. URL · 2. BODY (JSON schema) · 3. RESPONSE · 4. AUTH · 5. ƏLAQƏLİ API-lar · 6. Nə zaman istifadə edilməlidir · 7. Nə zaman istifadə etmək olmaz.

**Ümumi qeyd:** Səhifələnmiş cavablarda `model` — `PagedResultDto<T>` (items, pageNumber, pageSize, totalCount, totalPages). Lokalizasiya olan classifier-lərdə `name` / `description` çox vaxt `LocalizationDto[]` (languageId, text, key) formatındadır.

---

### GET — GetPeriodsAsync / GetPeriodByIdAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `GET api/Periods` və `GET api/Periods/{id}` |
| **2. BODY** | Yoxdur. List üçün query: pagination + PeriodPaginationQueryDto. |
| **3. RESPONSE** | Uğur (200): list = PagedResultDto<PeriodDto> (id, key, name[], description[], periodTime); by id = PeriodDto. |
| **4. AUTH** | AllowAnonymous. |
| **5. ƏLAQƏLİ** | [POST api/Periods](#post-createperiodasync), [DoctorCalendar/period-plan](DoctorCalendar.md#put-editperiodplanasync), [Doctors/schemas](Doctors.md#post-adddoctorschemaasync). |
| **6. İstifadə** | Müddət (dəqiqə) siyahısı — təqvim slotları və sxemlər üçün. |
| **7. İstifadə etmə** | Tək period üçün — GET by id. |

### POST — CreatePeriodAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `POST api/Periods` |
| **2. BODY** | PeriodCreateDto: `periodTime` (byte), `name` (CreateLocalizationDto[]), `description` (CreateLocalizationDto[]). |
| **3. RESPONSE** | Uğur (200). 400/403. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/Periods](#get-getperiodsasync). |
| **6. İstifadə** | Yeni period (məs. 15, 30 dəqiqə) əlavə etmək. |
| **7. İstifadə etmə** | Admin deyilsə. |

### PUT — UpdatePeriodAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `PUT api/Periods/{id}` |
| **2. BODY** | PeriodUpdateDto: `periodTime`, name/description. |
| **3. RESPONSE** | Uğur (200). 400/404/403. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/Periods/{id}](#get-getperiodbyidasync). |
| **6. İstifadə** | Müddəti və ya mətnləri dəyişmək. |
| **7. İstifadə etmə** | Admin deyilsə. |

---

[← Currencies](Currencies.md) | [API indeksi](../README.md) | [PaymentTypes →](PaymentTypes.md)
