# Currencies Controller

Bütün classifier endpointləri aşağıdakı ardıcıllıqla təsvir olunur: 1. URL · 2. BODY (JSON schema) · 3. RESPONSE · 4. AUTH · 5. ƏLAQƏLİ API-lar · 6. Nə zaman istifadə edilməlidir · 7. Nə zaman istifadə etmək olmaz.

**Ümumi qeyd:** Səhifələnmiş cavablarda `model` — `PagedResultDto<T>` (items, pageNumber, pageSize, totalCount, totalPages). Lokalizasiya olan classifier-lərdə `name` / `description` çox vaxt `LocalizationDto[]` (languageId, text, key) formatındadır.

---

### GET — GetCurrenciesAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `GET api/Currencies` |
| **2. BODY** | Yoxdur. Query: pagination + CurrencyPaginationQueryDto (əlavə filter varsa). |
| **3. RESPONSE** | Uğur (200): `model` = CurrencyDto[] (id, key, name[], description[], coefficent). |
| **4. AUTH** | AllowAnonymous. |
| **5. ƏLAQƏLİ** | [GET api/Currencies/{id}](#get-getcurrencybyidasync), [POST api/Currencies](#post-createcurrencyasync), [Calendar/period-plan](DoctorCalendar.md#put-editperiodplanasync) (price/currency). |
| **6. İstifadə** | Valyuta siyahısı (qiymət, təqvim və s. üçün). |
| **7. İstifadə etmə** | Tək valyuta üçün — GET by id. |

### GET — GetCurrencyByIdAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `GET api/Currencies/{id}` |
| **2. BODY** | Yoxdur. |
| **3. RESPONSE** | Uğur (200): `model` = CurrencyDto. 404: tapılmadı. |
| **4. AUTH** | AllowAnonymous. |
| **5. ƏLAQƏLİ** | [GET api/Currencies](#get-getcurrenciesasync), [PUT api/Currencies/{id}](#put-updatecurrencyasync). |
| **6. İstifadə** | Bir valyutanın məlumatı. |
| **7. İstifadə etmə** | Siyahı lazımdırsa — GET list. |

### POST — CreateCurrencyAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `POST api/Currencies` |
| **2. BODY** | CurrencyCreateDto: `coefficent` (number), `name` (CreateLocalizationDto[]), `description` (CreateLocalizationDto[]). |
| **3. RESPONSE** | Uğur (200). 400/403. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/Currencies](#get-getcurrenciesasync). |
| **6. İstifadə** | Yeni valyuta əlavə etmək. |
| **7. İstifadə etmə** | Admin deyilsə. |

### PUT — UpdateCurrencyAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `PUT api/Currencies/{id}` |
| **2. BODY** | CurrencyUpdateDto: `coefficent`, name/description (ClassifierDto əsaslı). |
| **3. RESPONSE** | Uğur (200). 400/404/403. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/Currencies/{id}](#get-getcurrencybyidasync). |
| **6. İstifadə** | Valyuta əmsalı və ya mətnləri dəyişmək. |
| **7. İstifadə etmə** | Admin deyilsə. |

---

[← Specialties](Specialties.md) | [API indeksi](../README.md) | [Periods →](Periods.md)
