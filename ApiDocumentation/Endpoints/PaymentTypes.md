# PaymentTypes Controller

Bütün classifier endpointləri aşağıdakı ardıcıllıqla təsvir olunur: 1. URL · 2. BODY (JSON schema) · 3. RESPONSE · 4. AUTH · 5. ƏLAQƏLİ API-lar · 6. Nə zaman istifadə edilməlidir · 7. Nə zaman istifadə etmək olmaz.

**Ümumi qeyd:** Səhifələnmiş cavablarda `model` — `PagedResultDto<T>` (items, pageNumber, pageSize, totalCount, totalPages). Lokalizasiya olan classifier-lərdə `name` / `description` çox vaxt `LocalizationDto[]` (languageId, text, key) formatındadır.

---

### GET — GetPaymentTypesAsync / GetPaymentTypeByIdAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `GET api/PaymentTypes` və `GET api/PaymentTypes/{id}` |
| **2. BODY** | Yoxdur. List: ClassifierPaginationQueryDto (pageNumber, pageSize, nameFilter, descriptionFilter). |
| **3. RESPONSE** | Uğur (200): list = PagedResultDto<PaymentTypeDto>; by id = PaymentTypeDto (id, key, name[], description[]). |
| **4. AUTH** | AllowAnonymous. |
| **5. ƏLAQƏLİ** | [POST api/PaymentTypes](#post-createpaymenttypeasync), [PUT api/PaymentTypes/{id}](#put-updatepaymenttypeasync). |
| **6. İstifadə** | Ödəniş növü siyahısı (nağd, kart, və s.). |
| **7. İstifadə etmə** | Tək növ üçün — GET by id. |

### POST — CreatePaymentTypeAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `POST api/PaymentTypes` |
| **2. BODY** | PaymentTypeCreateDto: ClassifierDto əsaslı (key, name[], description[]). |
| **3. RESPONSE** | Uğur (200). 400/403. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/PaymentTypes](#get-getpaymenttypesasync). |
| **6. İstifadə** | Yeni ödəniş növü əlavə etmək. |
| **7. İstifadə etmə** | Admin deyilsə. |

### PUT — UpdatePaymentTypeAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `PUT api/PaymentTypes/{id}` |
| **2. BODY** | PaymentTypeUpdateDto (ClassifierDto: key, name[], description[]). |
| **3. RESPONSE** | Uğur (200). 400/404/403. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/PaymentTypes/{id}](#get-getpaymenttypebyidasync). |
| **6. İstifadə** | Ödəniş növünün mətnlərini dəyişmək. |
| **7. İstifadə etmə** | Admin deyilsə. |

---

[← Periods](Periods.md) | [API indeksi](../README.md) | [PlanPaddingTypes →](PlanPaddingTypes.md)
