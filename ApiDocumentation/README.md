# API Documentation

Bu sənəd **MedAppointment** API-nin GET, POST və digər endpointlərinin siyahısı və bir-birinə keçidli təsvirini təqdim edir.

## Ümumi məlumat

- **Base URL:** `api/` (controller adı avtomatik əlavə olunur: `[controller]`)
- **Format:** JSON
- **Authentication:** JWT Bearer (əksər endpointlər üçün); `[AllowAnonymous]` olanlar istisnadır.

### Ümumi response zərfi (Result / Result&lt;T&gt;)

Əksər endpointlər aşağıdakı JSON strukturunda cavab qaytarır. Auth uğurlu olanda bəzən yalnız `accessToken` qaytarılır (aşağıda izah olunur).

**Uğursuz və ya data ilə cavab (CustomResult):**

```json
{
  "httpStatus": 200,
  "messages": [
    { "textCode": "ERR00001", "text": "Localized message", "exception": null }
  ],
  "model": { }
}
```

| Sahə | Məna |
|------|------|
| `httpStatus` | HTTP status kodu (rəqəm: 200, 400, 401, 403, 404, 500) |
| `messages` | Mesaj siyahısı; xəta zamanı `textCode` və `text` doldurulur |
| `model` | Yalnız `Result<T>` üçün; uğurda data obyekti (siyahı, əlavə, və s.) |

**Auth uğuru (SuccessAuthResult):** Login və Refresh uğurda response body yalnız `{ "accessToken": "..." }`, `RefreshToken` isə **cookie** ilə göndərilir.

## Sənəd strukturu

| Sənəd | Təsvir |
|-------|--------|
| [README.md](README.md) | Bu fayl — API indeksi |
| [Endpoints/Login.md](Endpoints/Login.md) | Login controller — giriş, refresh token |
| [Endpoints/Registration.md](Endpoints/Registration.md) | Registration controller — qeydiyyat |
| [Endpoints/AdminUsers.md](Endpoints/AdminUsers.md) | AdminUsers controller — istifadəçi siyahısı, detal, silmə |
| [Endpoints/Doctors.md](Endpoints/Doctors.md) | Doctors controller — həkimlər, qeydiyyat, təsdiq, ixtisaslar, sxemlər |
| [Endpoints/Languages.md](Endpoints/Languages.md) | Languages controller — dillər |
| [Endpoints/Specialties.md](Endpoints/Specialties.md) | Specialties controller — ixtisaslar |
| [Endpoints/Currencies.md](Endpoints/Currencies.md) | Currencies controller — valyutalar |
| [Endpoints/Periods.md](Endpoints/Periods.md) | Periods controller — periodlar |
| [Endpoints/PaymentTypes.md](Endpoints/PaymentTypes.md) | PaymentTypes controller — ödəniş növləri |
| [Endpoints/PlanPaddingTypes.md](Endpoints/PlanPaddingTypes.md) | PlanPaddingTypes controller — padding tipləri |
| [Endpoints/DoctorCalendar.md](Endpoints/DoctorCalendar.md) | DoctorCalendar controller — həftəlik təqvim, gün və period planları |

## Endpoint xülasəsi

### Auth
- **POST** [api/Login](Endpoints/Login.md#post-traditional-login) — giriş  
- **POST** [api/Login/refresh](Endpoints/Login.md#post-refresh-token) — refresh token  
- **POST** [api/Registration](Endpoints/Registration.md#post-traditional-client-registration) — qeydiyyat  

### Users & Doctors
- **GET** [api/AdminUsers](Endpoints/AdminUsers.md#get-getusersasync-adminusers-siyahısı) — istifadəçi siyahısı  
- **GET** [api/AdminUsers/{id}](Endpoints/AdminUsers.md#get-getuserbyidasync-adminusers-detallı) — istifadəçi detalı  
- **GET** [api/Doctors](Endpoints/Doctors.md#get-getallasync-doctors-siyahısı) — həkim siyahısı  
- **GET** [api/Doctors/{id}](Endpoints/Doctors.md#get-getbyidasync-doctor-detallı) — həkim detalı  
- **POST** [api/Doctors/register](Endpoints/Doctors.md#post-registertraditionaldoctorasync) — həkim qeydiyyatı  

### Classifiers
- Hər classifier üçün: **GET** siyahı, **GET** `{id}`, **POST** yaratmaq, **PUT** `{id}` yeniləmək (və bəzilərində **DELETE**).  
- [Languages](Endpoints/Languages.md) · [Specialties](Endpoints/Specialties.md) · [Currencies](Endpoints/Currencies.md) · [Periods](Endpoints/Periods.md) · [PaymentTypes](Endpoints/PaymentTypes.md) · [PlanPaddingTypes](Endpoints/PlanPaddingTypes.md).

### Calendar
- **GET** [api/DoctorCalendar/week](Endpoints/DoctorCalendar.md#get-getweeklycalendarasync) — həftəlik təqvim  
- **PUT** [api/DoctorCalendar/day-plan](Endpoints/DoctorCalendar.md#put-editdayplanasync), **PUT** [api/DoctorCalendar/period-plan](Endpoints/DoctorCalendar.md#put-editperiodplanasync) — gün və period planları  

---

Ətraflı təsvir, parametrlər və bir-birinə keçidlər üçün yuxarıdakı endpoint sənədlərinə baxın.
