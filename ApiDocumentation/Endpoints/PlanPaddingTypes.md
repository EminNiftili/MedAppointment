# PlanPaddingTypes Controller

Bütün classifier endpointləri aşağıdakı ardıcıllıqla təsvir olunur: 1. URL · 2. BODY (JSON schema) · 3. RESPONSE · 4. AUTH · 5. ƏLAQƏLİ API-lar · 6. Nə zaman istifadə edilməlidir · 7. Nə zaman istifadə etmək olmaz.

**Ümumi qeyd:** Səhifələnmiş cavablarda `model` — `PagedResultDto<T>` (items, pageNumber, pageSize, totalCount, totalPages). Lokalizasiya olan classifier-lərdə `name` / `description` çox vaxt `LocalizationDto[]` (languageId, text, key) formatındadır.

---

### GET — GetPlanPaddingTypesAsync / GetPlanPaddingTypeByIdAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `GET api/PlanPaddingTypes` və `GET api/PlanPaddingTypes/{id}` |
| **2. BODY** | Yoxdur. List: PlanPaddingTypePaginationQueryDto (pagination + filter). |
| **3. RESPONSE** | Uğur (200): PlanPaddingTypeDto (id, key, name[], description[], paddingPosition, paddingTime). |
| **4. AUTH** | AllowAnonymous. |
| **5. ƏLAQƏLİ** | [POST api/PlanPaddingTypes](#post-createplanpaddingtypeasync), [Doctors/schemas](Doctors.md#post-adddoctorschemaasync) (daySchemas.planPaddingTypeId). |
| **6. İstifadə** | Slot arası padding tipləri — həkim sxemlərində istifadə olunur. |
| **7. İstifadə etmə** | Tək tip üçün — GET by id. |

### POST — CreatePlanPaddingTypeAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `POST api/PlanPaddingTypes` |
| **2. BODY** | PlanPaddingTypeCreateDto: `paddingPosition` (enum), `paddingTime` (byte), `name`[], `description`[]. |
| **3. RESPONSE** | Uğur (200). 400/403. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/PlanPaddingTypes](#get-getplanpaddingtypesasync). |
| **6. İstifadə** | Yeni padding tipi (məs. növbələr arası fasilə) əlavə etmək. |
| **7. İstifadə etmə** | Admin deyilsə. |

### PUT — UpdatePlanPaddingTypeAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `PUT api/PlanPaddingTypes/{id}` |
| **2. BODY** | PlanPaddingTypeUpdateDto: paddingPosition, paddingTime, name[], description[]. |
| **3. RESPONSE** | Uğur (200). 400/404/403. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/PlanPaddingTypes/{id}](#get-getplanpaddingtypebyidasync). |
| **6. İstifadə** | Padding müddəti və ya mövqeyini dəyişmək. |
| **7. İstifadə etmə** | Admin deyilsə. |

---

[← PaymentTypes](PaymentTypes.md) | [API indeksi](../README.md) | [DoctorCalendar →](DoctorCalendar.md)
