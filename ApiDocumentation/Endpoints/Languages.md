# Languages Controller

Bütün classifier endpointləri aşağıdakı ardıcıllıqla təsvir olunur: 1. URL · 2. BODY (JSON schema) · 3. RESPONSE · 4. AUTH · 5. ƏLAQƏLİ API-lar · 6. Nə zaman istifadə edilməlidir · 7. Nə zaman istifadə etmək olmaz.

**Ümumi qeyd:** Səhifələnmiş cavablarda `model` — `PagedResultDto<T>` (items, pageNumber, pageSize, totalCount, totalPages). Lokalizasiya olan classifier-lərdə `name` / `description` çox vaxt `LocalizationDto[]` (languageId, text, key) formatındadır.

---

### GET — GetLanguagesAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `GET api/Languages` |
| **2. BODY** | Yoxdur. Query: `pageNumber`, `pageSize`, `nameFilter` (optional), `isDefaultFilter` (optional, bool). |
| **3. RESPONSE** | Uğur (200): `model` = siyahı (items: LanguageDto[], totalCount, totalPages, …). LanguageDto: `id`, `name`, `isDefault`. Uğursuz: ümumi Result zərfi. |
| **4. AUTH** | `[AllowAnonymous]` — rol tələbi yoxdur. |
| **5. ƏLAQƏLİ** | [GET api/Languages/{id}](#get-getlanguagebyidasync), [POST api/Languages](#post-createlanguageasync). |
| **6. İstifadə** | Dil siyahısını səhifələmək, filtrləmək; UI-da dil seçimi və digər classifier create/update üçün languageId. |
| **7. İstifadə etmə** | Tək dil lazımdırsa — GET by id. Yeni dil yaratmaq üçün — POST. |

### GET — GetLanguageByIdAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `GET api/Languages/{id}` (id: long) |
| **2. BODY** | Yoxdur. |
| **3. RESPONSE** | Uğur (200): `model` = LanguageDto (`id`, `name`, `isDefault`). 404: dil tapılmadı. |
| **4. AUTH** | AllowAnonymous. |
| **5. ƏLAQƏLİ** | [GET api/Languages](#get-getlanguagesasync), [PUT api/Languages/{id}](#put-updatelanguageasync), [DELETE api/Languages/{id}](#delete-deletelanguageasync). |
| **6. İstifadə** | Bir dilin məlumatını göstərmək və ya redaktə/silmə əvvəli yoxlamaq. |
| **7. İstifadə etmə** | Siyahı lazımdırsa — GET list. |

### POST — CreateLanguageAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `POST api/Languages` |
| **2. BODY** | `{ "name": "string", "isDefault": false }` (JSON schema: name required, isDefault boolean). |
| **3. RESPONSE** | Uğur (200): Result zərfi; model boş və ya yaradılan dil. 400: validasiya. |
| **4. AUTH** | **SystemAdmin** — yalnız admin yeni dil əlavə edə bilər. |
| **5. ƏLAQƏLİ** | [GET api/Languages](#get-getlanguagesasync) — siyahı; digər classifier create/update üçün languageId. |
| **6. İstifadə** | Sistemə yeni dil əlavə etmək (çoxdilli mətnlər üçün). |
| **7. İstifadə etmə** | Rol admin deyilsə. Mövcud dili dəyişmək üçün — PUT. |

### PUT — UpdateLanguageAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `PUT api/Languages/{id}` (id: long) |
| **2. BODY** | `{ "name": "string", "isDefault": false }` |
| **3. RESPONSE** | Uğur (200). 400/404: validasiya və ya tapılmadı. 403: SystemAdmin deyil. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/Languages/{id}](#get-getlanguagebyidasync) — mövcud məlumat. |
| **6. İstifadə** | Dilin adını və ya default flag-ini dəyişmək. |
| **7. İstifadə etmə** | Admin deyilsə. Yeni dil — POST. |

### DELETE — DeleteLanguageAsync

| # | Məzmun |
|---|--------|
| **1. URL** | `DELETE api/Languages/{id}` (id: long) |
| **2. BODY** | Yoxdur. |
| **3. RESPONSE** | Uğur (200). 400: məs. dil istifadədədirsə silinməyə bilər. 403/404: admin deyil / tapılmadı. |
| **4. AUTH** | **SystemAdmin**. |
| **5. ƏLAQƏLİ** | [GET api/Languages](#get-getlanguagesasync) — siyahı. |
| **6. İstifadə** | Artıq lazım olmayan dili sistemdən çıxartmaq (ehtiyatla; asılılıqlar yoxlanılmalıdır). |
| **7. İstifadə etmə** | Admin deyilsə. Default dil və ya istifadədə olan dil ola bilər — biznes qaydalarına uyğun yoxlayın. |

---

[← Doctors](Doctors.md) | [API indeksi](../README.md) | [Specialties →](Specialties.md)
