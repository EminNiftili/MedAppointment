# Specialties Controller

Bütün classifier endpointləri aşağıdakı ardıcıllıqla təsvir olunur: 1. URL · 2. BODY (JSON schema) · 3. RESPONSE · 4. AUTH · 5. ƏLAQƏLİ API-lar · 6. Nə zaman istifadə edilməlidir · 7. Nə zaman istifadə etmək olmaz.

**Ümumi qeyd:** Səhifələnmiş cavablarda `model` — `PagedResultDto<T>` (items, pageNumber, pageSize, totalCount, totalPages). Lokalizasiya olan classifier-lərdə `name` / `description` çox vaxt `LocalizationDto[]` (languageId, text, key) formatındadır.

---

### GET — GetSpecialtiesAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `GET api/Specialties` |
| **2. BODY** | Yoxdur. Query: `pageNumber`, `pageSize`, `nameFilter`, `descriptionFilter` (ClassifierPaginationQueryDto). |
| **3. RESPONSE** | Uğur (200): `model` = səhifələnmiş SpecialtyDto siyahısı (id, key, name[], description[]). |
| **4. AUTH** | AllowAnonymous. |
| **5. ƏLAQƏLİ** | [GET api/Specialties/{id}](#get-getspecialtybyidasync), [POST api/Specialties](#post-createspecialtyasync), [Doctors/specialties](Doctors.md#post-adddoctorspecialtyasync). |
| **6. İstifadə** | İxtisas siyahısı (həkim qeydiyyatı, filter, admin panel). |
| **7. İstifadə etmə** | Tək ixtisas üçün — GET by id. |

### GET — GetSpecialtyByIdAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `GET api/Specialties/{id}` |
| **2. BODY** | Yoxdur. |
| **3. RESPONSE** | Uğur (200): `model` = SpecialtyDto (id, key, name[], description[]). |
| **4. AUTH** | AllowAnonymous. |
| **5. ƏLAQƏLİ** | [GET api/Specialties](#get-getspecialtiesasync), [PUT api/Specialties/{id}](#put-updatespecialtyasync), [Doctors — specialties](Doctors.md#post-adddoctorspecialtyasync). |
| **6. İstifadə** | Bir ixtisasın detallı məlumatı. |
| **7. İstifadə etmə** | Siyahı lazımdırsa — GET list. |

### POST — CreateSpecialtyAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `POST api/Specialties` |
| **2. BODY** | SpecialtyCreateDto: `key`, `name` (CreateLocalizationDto[]: languageId, text), `description` (eyni struktur). |
| **3. RESPONSE** | Uğur (200). 400: validasiya. 403: SystemAdmin deyil. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/Specialties](#get-getspecialtiesasync), [api/Languages](Languages.md#get-getlanguagesasync) — dillər üçün. |
| **6. İstifadə** | Yeni ixtisas (məs. tibbi ixtisas) əlavə etmək. |
| **7. İstifadə etmə** | Admin deyilsə. Yeniləmək üçün — PUT. |

### PUT — UpdateSpecialtyAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `PUT api/Specialties/{id}` |
| **2. BODY** | ClassifierDto əsaslı: key, name[], description[] (LocalizationDto). |
| **3. RESPONSE** | Uğur (200). 400/404/403. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/Specialties/{id}](#get-getspecialtybyidasync). |
| **6. İstifadə** | İxtisasın ad/təsvirini (dillər üzrə) dəyişmək. |
| **7. İstifadə etmə** | Admin deyilsə. |

---

[← Languages](Languages.md) | [API indeksi](../README.md) | [Currencies →](Currencies.md)
