# DoctorCalendar Controller

Hər endpoint: 1. URL · 2. BODY (JSON schema) · 3. RESPONSE · 4. AUTH · 5. ƏLAQƏLİ API-lar · 6. Nə zaman istifadə edilməlidir · 7. Nə zaman istifadə etmək olmaz.

---

## GET — GetWeeklyCalendarAsync

### 1. URL

```
GET api/DoctorCalendar/week
```

### 2. BODY

GET sorğusu üçün body yoxdur. Parametrlər **query string** ilə göndərilir.

**Query (JSON schema ekvivalentı):**

```json
{
  "type": "object",
  "required": [ "doctorId", "weekStartDate" ],
  "properties": {
    "doctorId": { "type": "integer" },
    "weekStartDate": { "type": "string", "format": "date-time" }
  }
}
```

### 3. RESPONSE tipləri və mənası

**Uğur (200):** `model` — DoctorCalendarWeekResponseDto.

```json
{
  "httpStatus": 200,
  "messages": [],
  "model": {
    "doctorId": 1,
    "weekStartDate": "2025-02-17T00:00:00",
    "weekEndDate": "2025-02-23T00:00:00",
    "days": [
      {
        "date": "2025-02-17T00:00:00",
        "dayOfWeek": 1,
        "isClosed": false,
        "periods": [
          {
            "periodPlanId": 1,
            "periodStart": "09:00:00",
            "periodStop": "09:30:00",
            "pricePerPeriod": 50.00,
            "currencyId": 1,
            "currencyKey": "AZN",
            "periodId": 1,
            "periodTimeMinutes": 30,
            "specialtyId": 1,
            "isBusy": false,
            "isOnlineService": true,
            "isOnSiteService": true
          }
        ]
      }
    ]
  }
}
```

| Sahə | Məna |
|------|------|
| days | Həftənin hər günü üçün tarix, dayOfWeek, isClosed, periods (slot siyahısı) |
| periods | Hər slot: periodPlanId, başlanğıc/bitmə vaxtı, qiymət, valyuta, period müddəti, ixtisas, isBusy, online/onSite |

**Uğursuzluq (400/404):** Ümumi Result zərfi; məs. doctorId və ya tarix etibarsızdır.

### 4. AUTH

- **Rol tələbi:** yoxdur (`[AllowAnonymous]`).
- **Məntiq:** Həftəlik təqvim ictimai (pasientlər həkimin məşğuliyyətini görmək üçün); token tələb olunmur.

### 5. ƏLAQƏLİ API-lar

- [PUT api/DoctorCalendar/day-plan](#put-editdayplanasync) — günü "bağlı" etmək və ya ixtisası dəyişmək.
- [PUT api/DoctorCalendar/period-plan](#put-editperiodplanasync) — slot qiyməti, məşğul statusu və s. dəyişmək.
- [POST api/Doctors/calendar/fill-from-weekly-schema](Doctors.md#post-createdayplansfromweeklyschemabyidasync) — həftəni sxemdən doldurmaq.
- [POST api/Doctors/schemas](Doctors.md#post-adddoctorschemaasync) — həftəlik sxem (şablon) yaratmaq.

### 6. Nə zaman istifadə edilməlidir

- Pasient həkimin bir həftəlik məşğuliyyətini və slotlarını görmək istəyəndə.
- Randevu səhifəsində "bu həftə üçün boş vaxtlar" göstərmək üçün.
- Həkim öz təqvimini yoxlayanda və ya redaktə etməzdən əvvəl məlumatı görmək üçün.

### 7. Nə zaman istifadə etmək olmaz

- Təqvimi dəyişdirmək (redaktə) üçün — day-plan və ya period-plan PUT istifadə edin.
- Həftəni avtomatik doldurmaq üçün — fill-from-weekly-schema istifadə edin; bu endpoint yalnız oxumaq üçündür.

---

## PUT — EditDayPlanAsync

### 1. URL

```
PUT api/DoctorCalendar/day-plan
```

### 2. BODY (JSON Schema)

```json
{
  "type": "object",
  "required": [ "dayPlanId", "doctorId", "specialtyId", "isClosed" ],
  "properties": {
    "dayPlanId": { "type": "integer" },
    "doctorId": { "type": "integer" },
    "specialtyId": { "type": "integer" },
    "isClosed": { "type": "boolean" }
  }
}
```

### 3. RESPONSE tipləri və mənası

**Uğur (200):** Ümumi Result zərfi; `model` boş ola bilər — gün planı yeniləndi.

**Uğursuzluq (400/404):** dayPlanId və ya doctorId/specialtyId etibarsızdır; Result zərfində `messages` doldurulur.

### 4. AUTH

- **Rol:** **Doctor** (və ya SystemAdmin).
- **Məntiq:** Yalnız həkim (və ya admin) öz təqvimində günü "açıq/bağlı" edə və ya həmin gün üçün ixtisası dəyişə bilər.

### 5. ƏLAQƏLİ API-lar

- [GET api/DoctorCalendar/week](#get-getweeklycalendarasync) — dayPlanId və gün məlumatını əldə etmək.
- [POST api/Doctors/schemas](Doctors.md#post-adddoctorschemaasync) — sxem yaratmaq (sonradan bu günlər doldurula bilər).
- [PUT api/DoctorCalendar/period-plan](#put-editperiodplanasync) — həmin gündəki slotları (period planları) ayrıca redaktə etmək.

### 6. Nə zaman istifadə edilməlidir

- Həkim müəyyən günü "bağlı" (isClosed = true) elan etmək istəyəndə.
- Gün üçün göstərilən ixtisası (specialtyId) dəyişmək istəyəndə.
- Həftəlik təqvim artıq doldurulub, yalnız gün səviyyəsində dəyişiklik lazımdır.

### 7. Nə zaman istifadə etmək olmaz

- Rol Doctor (və ya admin) deyilsə.
- Slot səviyyəsində qiymət, məşğul statusu və s. dəyişmək üçün — [PUT api/DoctorCalendar/period-plan](#put-editperiodplanasync) istifadə edin.
- Yeni gün planları yaratmaq üçün — əvvəlcə fill-from-weekly-schema və ya sxem əsasında doldurmaq lazımdır; bu endpoint mövcud günü redaktə edir.

---

## PUT — EditPeriodPlanAsync

### 1. URL

```
PUT api/DoctorCalendar/period-plan
```

### 2. BODY (JSON Schema)

```json
{
  "type": "object",
  "required": [
    "periodPlanId", "doctorId", "periodStart", "periodStop",
    "isOnlineService", "isOnSiteService", "pricePerPeriod", "currencyId", "isBusy"
  ],
  "properties": {
    "periodPlanId": { "type": "integer" },
    "doctorId": { "type": "integer" },
    "periodStart": { "type": "string", "description": "TimeSpan, e.g. 09:00:00" },
    "periodStop": { "type": "string", "description": "TimeSpan" },
    "isOnlineService": { "type": "boolean" },
    "isOnSiteService": { "type": "boolean" },
    "pricePerPeriod": { "type": "number" },
    "currencyId": { "type": "integer" },
    "isBusy": { "type": "boolean" }
  }
}
```

### 3. RESPONSE tipləri və mənası

**Uğur (200):** Result zərfi; slot (period plan) yeniləndi.

**Uğursuzluq (400/404):** periodPlanId və ya doctorId/currencyId etibarsızdır; validasiya xətaları `messages`-da.

### 4. AUTH

- **Rol:** **Doctor** (və ya SystemAdmin).
- **Məntiq:** Həkim öz təqvimindəki slotun qiymətini, online/onSite flaglərini və məşğul statusunu dəyişə bilər.

### 5. ƏLAQƏLİ API-lar

- [GET api/DoctorCalendar/week](#get-getweeklycalendarasync) — periodPlanId və slot məlumatını əldə etmək.
- [GET api/Currencies](Currencies.md#get-getcurrenciesasync) — currencyId seçmək üçün.
- [GET api/Periods](Periods.md#get-getperiodsasync--get-periodbyidasync) — period müddəti məlumatı üçün.
- [PUT api/DoctorCalendar/day-plan](#put-editdayplanasync) — gün səviyyəsində bağlı/açıq və ixtisas dəyişdirmək.

### 6. Nə zaman istifadə edilməlidir

- Həkim slotun qiymətini və ya valyutasını dəyişmək istəyəndə.
- Slotun online/onSite xidmət növünü dəyişmək üçün.
- Randevu tutulduqda slotu "məşğul" (isBusy = true) etmək və ya ləğv edəndə boşaltmaq üçün (əgər bu API ilə idarə olunursa).

### 7. Nə zaman istifadə etmək olmaz

- Rol Doctor (və ya admin) deyilsə.
- Bütün günü bağlamaq və ya ixtisası dəyişmək üçün — [PUT api/DoctorCalendar/day-plan](#put-editdayplanasync).
- Yeni slot yaratmaq üçün — bu endpoint mövcud period planı redaktə edir; yeni slotlar sxem/fill-from-weekly-schema ilə yaranır.

---

[← PlanPaddingTypes](PlanPaddingTypes.md) | [API indeksi](../README.md) | [Doctors](Doctors.md)
