# Doctors Controller

Hər endpoint aşağıdakı ardıcıllıqla təsvir olunur: 1. URL · 2. BODY (JSON schema) · 3. RESPONSE · 4. AUTH · 5. ƏLAQƏLİ API-lar · 6. Nə zaman istifadə edilməlidir · 7. Nə zaman istifadə etmək olmaz.

---

## GET — GetAllAsync (Doctors siyahısı)

### 1. URL

```
GET api/Doctors
```

### 2. BODY

Yoxdur. **Query:** `pageNumber`, `pageSize` (pagination).

### 3. RESPONSE tipləri və mənası

**Uğur (200):** `model` — `PagedResultDto<DoctorDto>`.

```json
{
  "httpStatus": 200,
  "messages": [],
  "model": {
    "items": [
      {
        "id": 1,
        "name": "string",
        "surname": "string",
        "imagePath": "string | null",
        "title": "string",
        "description": "string",
        "isConfirm": true,
        "specialties": [
          { "id": 1, "key": "...", "name": [], "description": [], "isConfirm": true }
        ]
      }
    ],
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 50,
    "totalPages": 3
  }
}
```

**403:** SystemAdmin deyil.

### 4. AUTH

- **Rol:** **SystemAdmin**.
- **Məntiq:** Bütün həkimlərin siyahısı admin üçündür (təsdiq və idarəetmə üçün).

### 5. ƏLAQƏLİ API-lar

- [GET api/Doctors/{id}](#get-getbyidasync-doctor-detallı) — bir həkimin detallı məlumatı.
- [POST api/Doctors/register](#post-registertraditionaldoctorasync) — yeni həkim qeydiyyatı.
- [PUT api/Doctors/confirm/{doctorId}](#put-confirmdoctorasync) — həkimi təsdiq etmək.

### 6. Nə zaman istifadə edilməlidir

- Admin paneldə həkim siyahısını idarə etmək, təsdiq etmək üçün.
- Səhifələnmiş həkim siyahısı göstərmək üçün.

### 7. Nə zaman istifadə etmək olmaz

- SystemAdmin deyilsə.
- Tək həkim məlumatı üçün — [GET api/Doctors/{id}](#get-getbyidasync-doctor-detallı).

---

## GET — GetByIdAsync (Doctor detallı)

### 1. URL

```
GET api/Doctors/{id}
```

`id`: long.

### 2. BODY

Yoxdur.

### 3. RESPONSE tipləri və mənası

**Uğur (200):** `model` — `DoctorDto` (ad, soyad, title, description, isConfirm, specialties).

**404:** Həkim tapılmadı. **403:** SystemAdmin deyil.

### 4. AUTH

- **Rol:** **SystemAdmin**.
- **Məntiq:** Həkim detalları admin üçün (təsdiq/specialty idarəetmə üçün).

### 5. ƏLAQƏLİ API-lar

- [GET api/Doctors](#get-getallasync-doctors-siyahısı) — siyahıdan id seçmək.
- [PUT api/Doctors/confirm/{doctorId}](#put-confirmdoctorasync) — bu həkimi təsdiq etmək.
- [POST api/Doctors/{doctorId}/specialties](#post-adddoctorspecialtyasync) — ixtisas əlavə etmək.

### 6. Nə zaman istifadə edilməlidir

- Siyahıdan seçilmiş həkimin tam məlumatını göstərmək.
- Təsdiq / ixtisas əməliyyatlarından əvvəl məlumatı yoxlamaq.

### 7. Nə zaman istifadə etmək olmaz

- SystemAdmin deyilsə.
- Siyahı lazımdırsa — [GET api/Doctors](#get-getallasync-doctors-siyahısı).

---

## POST — RegisterTraditionalDoctorAsync

### 1. URL

```
POST api/Doctors/register
```

### 2. BODY (JSON Schema)

```json
{
  "type": "object",
  "required": [ "user", "specialties", "title", "description" ],
  "properties": {
    "user": {
      "type": "object",
      "required": [ "name", "surname", "fatherName", "birthDate", "email", "phoneNumber", "password" ],
      "properties": {
        "name": { "type": "string" },
        "surname": { "type": "string" },
        "fatherName": { "type": "string" },
        "birthDate": { "type": "string", "format": "date-time" },
        "email": { "type": "string" },
        "phoneNumber": { "type": "string" },
        "password": { "type": "string" }
      }
    },
    "specialties": { "type": "array", "items": { "type": "integer" } },
    "title": {
      "type": "array",
      "items": { "type": "object", "required": [ "languageId", "text" ], "properties": { "languageId": { "type": "integer" }, "text": { "type": "string" } }
    },
    "description": {
      "type": "array",
      "items": { "type": "object", "required": [ "languageId", "text" ], "properties": { "languageId": { "type": "integer" }, "text": { "type": "string" } }
    }
  }
}
```

### 3. RESPONSE tipləri və mənası

**Uğur (200):** Ümumi Result zərfi; `model` boş və ya yaradılan məlumat ola bilər.

**400:** Validasiya (email artıq mövcud, və s.). **403:** Anonymous olduğu üçün adətən 403 yoxdur; uğursuzluq 400 ilə qaytarılır.

### 4. AUTH

- **Rol:** yoxdur (`[AllowAnonymous]`).
- **Məntiq:** Həkim özünü qeydiyyatdan keçirir; admin təsdiqindən sonra aktiv olar.

### 5. ƏLAQƏLİ API-lar

- [POST api/Login](Login.md#post-traditional-login) — qeydiyyatdan sonra giriş.
- [GET api/Doctors](#get-getallasync-doctors-siyahısı) — admin üçün siyahı.
- [GET api/Specialties](Specialties.md#get-getspecialtiesasync) — ixtisas siyahısı (title/description üçün dillər [api/Languages](Languages.md#get-getlanguagesasync)).

### 6. Nə zaman istifadə edilməlidir

- Yeni həkim öz məlumatını və ixtisaslarını qeydiyyata alanda.
- Client qeydiyyatı deyil, həkim qeydiyyatı üçün.

### 7. Nə zaman istifadə etmək olmaz

- Adi istifadəçi (pasient) qeydiyyatı üçün — [POST api/Registration](Registration.md#post-traditional-client-registration).
- Artıq qeydiyyatlı həkim məlumatını dəyişmək üçün — (müvafiq update endpoint varsa o istifadə olunmalıdır).

---

## PUT — ConfirmDoctorAsync

### 1. URL

```
PUT api/Doctors/confirm/{doctorId}
```

### 2. BODY

Yoxdur.

### 3. RESPONSE tipləri və mənası

**Uğur (200):** Result zərfi; həkim təsdiq edildi.

**404:** Həkim tapılmadı. **403:** SystemAdmin deyil.

### 4. AUTH

- **Rol:** **SystemAdmin**.
- **Məntiq:** Gözləyən həkimləri təsdiq etmək üçün.

### 5. ƏLAQƏLİ API-lar

- [GET api/Doctors/{id}](#get-getbyidasync-doctor-detallı) — təsdiq etməzdən əvvəl məlumatı görmək.
- [PUT api/Doctors/confirmSpecialty/{doctorId}/{specialtyId}](Specialties.md) — həkim ixtisasını ayrıca təsdiq etmək.

### 6. Nə zaman istifadə edilməlidir

- Admin qeydiyyatdan keçmiş həkimi "təsdiqlənmiş" etmək istəyəndə.
- Həkim profilinin ictimai göstərilməsinə icazə vermək üçün.

### 7. Nə zaman istifadə etmək olmaz

- SystemAdmin deyilsə.
- Yalnız ixtisas təsdiqi lazımdırsa — confirmSpecialty istifadə edin.

---

## PUT — ConfirmDoctorSpecialty

### 1. URL

```
PUT api/Doctors/confirmSpecialty/{doctorId}/{specialtyId}
```

### 2. BODY

Yoxdur.

### 3. RESPONSE

Uğur: 200, Result zərfi. **404/403:** Müvafiq səhv kodları.

### 4. AUTH

- **Rol:** **SystemAdmin**.
- **Məntiq:** Həkimin konkret ixtisasını təsdiq etmək.

### 5. ƏLAQƏLİ API-lar

- [GET api/Doctors/{id}](#get-getbyidasync-doctor-detallı) — həkimin ixtisaslarını görmək.
- [GET api/Specialties](Specialties.md#get-getspecialtiesasync) — ixtisas siyahısı.

### 6. Nə zaman istifadə edilməlidir

- Həkim artıq təsdiqlənib, amma əlavə ixtisası ayrıca təsdiq etmək lazımdır.

### 7. Nə zaman istifadə etmək olmaz

- Bütün həkimi təsdiq etmək üçün — [PUT api/Doctors/confirm/{doctorId}](#put-confirmdoctorasync). SystemAdmin deyilsə.

---

## POST — AddDoctorSpecialtyAsync

### 1. URL

```
POST api/Doctors/{doctorId}/specialties
```

### 2. BODY (JSON Schema)

```json
{
  "type": "object",
  "required": [ "specialtyId", "isConfirmed" ],
  "properties": {
    "specialtyId": { "type": "integer" },
    "isConfirmed": { "type": "boolean" }
  }
}
```

### 3. RESPONSE

Uğur: 200. **400:** Validasiya. **403:** SystemAdmin deyil. **404:** Doctor/specialty tapılmadı.

### 4. AUTH

- **Rol:** **SystemAdmin**.
- **Məntiq:** Həkimə yeni ixtisas əlavə etmək (təsdiqli və ya təsdiqsiz).

### 5. ƏLAQƏLİ API-lar

- [GET api/Doctors/{id}](#get-getbyidasync-doctor-detallı) — mövcud ixtisasları görmək.
- [GET api/Specialties](Specialties.md#get-getspecialtiesasync) — mövcud ixtisaslar.
- [DELETE api/Doctors/{doctorId}/specialties/{specialtyId}](#delete-removedoctorspecialtyasync) — ixtisası çıxartmaq.

### 6. Nə zaman istifadə edilməlidir

- Admin həkimə əlavə ixtisas təyin etmək istəyəndə.

### 7. Nə zaman istifadə etmək olmaz

- SystemAdmin və ya həmin həkim (Doctor) deyilsə — rol siyasətinə görə yalnız admin ola bilər. İxtisası silmək üçün — DELETE endpoint.

---

## DELETE — RemoveDoctorSpecialtyAsync

### 1. URL

```
DELETE api/Doctors/{doctorId}/specialties/{specialtyId}
```

### 2. BODY

Yoxdur.

### 3. RESPONSE

Uğur: 200. **404/403:** Müvafiq səhvlər.

### 4. AUTH

- **Rol:** **Doctor** və ya **SystemAdmin**.
- **Məntiq:** Həkim öz ixtisasını və ya admin hər kəsin ixtisasını silə bilər.

### 5. ƏLAQƏLİ API-lar

- [GET api/Doctors/{id}](#get-getbyidasync-doctor-detallı) — ixtisasları görmək.
- [POST api/Doctors/{doctorId}/specialties](#post-adddoctorspecialtyasync) — ixtisas əlavə etmək.

### 6. Nə zaman istifadə edilməlidir

- Həkim özündən və ya admin həkimdən ixtisası çıxartmaq üçün.

### 7. Nə zaman istifadə etmək olmaz

- Rol Doctor və ya SystemAdmin deyilsə.

---

## POST — AddDoctorSchemaAsync

### 1. URL

```
POST api/Doctors/schemas
```

### 2. BODY (JSON Schema)

```json
{
  "type": "object",
  "required": [ "doctorId", "name", "colorHex", "daySchemas" ],
  "properties": {
    "doctorId": { "type": "integer" },
    "name": { "type": "string" },
    "colorHex": { "type": "string", "description": "RGBA hex, e.g. #RRGGBBAA" },
    "daySchemas": {
      "type": "array",
      "items": {
        "type": "object",
        "required": [ "specialtyId", "periodId", "dayOfWeek", "openTime", "periodCount", "isClosed", "isOnlineService", "isOnSiteService" ],
        "properties": {
          "specialtyId": { "type": "integer" },
          "periodId": { "type": "integer" },
          "planPaddingTypeId": { "type": [ "integer", "null" ] },
          "dayOfWeek": { "type": "integer", "description": "1=Monday..7=Sunday" },
          "openTime": { "type": "string", "description": "time span" },
          "periodCount": { "type": "integer" },
          "isClosed": { "type": "boolean" },
          "isOnlineService": { "type": "boolean" },
          "isOnSiteService": { "type": "boolean" },
          "dayBreaks": {
            "type": "array",
            "items": {
              "type": "object",
              "properties": {
                "name": { "type": [ "string", "null" ] },
                "isVisible": { "type": "boolean" },
                "startTime": { "type": "string" },
                "endTime": { "type": "string" }
              }
            }
          }
        }
      }
    }
  }
}
```

### 3. RESPONSE

Uğur: 200. **400:** Validasiya. **403:** Doctor (və ya admin) deyil.

### 4. AUTH

- **Rol:** **Doctor** (və ya SystemAdmin).
- **Məntiq:** Həkim öz həftəlik iş cədvəli sxemini yaradır.

### 5. ƏLAQƏLİ API-lar

- [GET api/DoctorCalendar/week](DoctorCalendar.md#get-getweeklycalendarasync) — təqvimi görmək.
- [POST api/Doctors/calendar/fill-from-weekly-schema](#post-createdayplansfromweeklyschemabyidasync) — bu sxemdən gün planları yaratmaq.
- [PUT api/DoctorCalendar/day-plan](DoctorCalendar.md#put-editdayplanasync) — gün planı redaktə.

### 6. Nə zaman istifadə edilməlidir

- Həkim həftəlik iş sxemi (şablon) əlavə etmək istəyəndə.
- Sonradan bu sxemdən konkret həftə üçün gün planları doldurmaq üçün.

### 7. Nə zaman istifadə etmək olmaz

- Rol Doctor deyilsə. Bir həftənin konkret günlərini doldurmaq üçün — fill-from-weekly-schema və ya day-plan redaktə.

---

## POST — CreateDayPlansFromWeeklySchemaByIdAsync

### 1. URL

```
POST api/Doctors/calendar/fill-from-weekly-schema
```

### 2. BODY (JSON Schema)

```json
{
  "type": "object",
  "required": [ "weeklySchemaId", "startDate", "currencyId", "pricePerPeriod" ],
  "properties": {
    "weeklySchemaId": { "type": "integer" },
    "startDate": { "type": "string", "format": "date-time", "description": "Həftənin başlanğıcı (Monday)" },
    "currencyId": { "type": "integer" },
    "pricePerPeriod": { "type": "number" }
  }
}
```

### 3. RESPONSE

Uğur: 200. **400/404:** Sxem tapılmadı və ya validasiya.

### 4. AUTH

- **Rol:** **Doctor**.
- **Məntiq:** Mövcud həftəlik sxemdən (şablondan) bir həftə üçün gün planları yaradılır.

### 5. ƏLAQƏLİ API-lar

- [POST api/Doctors/schemas](#post-adddoctorschemaasync) — sxem yaratmaq.
- [GET api/DoctorCalendar/week](DoctorCalendar.md#get-getweeklycalendarasync) — yaranan təqvimi görmək.
- [PUT api/DoctorCalendar/day-plan](DoctorCalendar.md#put-editdayplanasync) — gün planını sonradan dəyişmək.

### 6. Nə zaman istifadə edilməlidir

- Həkim bir həftə üçün sxem əsasında təqvim slotlarını avtomatik doldurmaq istəyəndə.
- Eyni sxemi fərqli həftələrə tətbiq etmək üçün.

### 7. Nə zaman istifadə etmək olmaz

- Rol Doctor deyilsə. Sxem yoxdursa əvvəlcə [POST api/Doctors/schemas](#post-adddoctorschemaasync). Tək gün redaktə üçün — [PUT api/DoctorCalendar/day-plan](DoctorCalendar.md#put-editdayplanasync).

---

[← AdminUsers](AdminUsers.md) | [API indeksi](../README.md) | [Languages](Languages.md) | [Specialties](Specialties.md) | [DoctorCalendar](DoctorCalendar.md)
