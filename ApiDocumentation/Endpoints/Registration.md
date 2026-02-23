# Registration Controller

Bu səhifə **Registration** controllerinin endpointlərini aşağıdakı strukturda təsvir edir: URL, BODY (JSON schema), RESPONSE, AUTH, ƏLAQƏLİ API-lar, Nə zaman istifadə edilməlidir, Nə zaman istifadə etmək olmaz.

---

## POST — Traditional (Client) Registration

### 1. URL

```
POST api/Registration
```

### 2. BODY (JSON Schema)

```json
{
  "type": "object",
  "required": [ "name", "surname", "fatherName", "birthDate", "email", "phoneNumber", "password" ],
  "properties": {
    "name": { "type": "string" },
    "surname": { "type": "string" },
    "fatherName": { "type": "string" },
    "birthDate": { "type": "string", "format": "date-time" },
    "email": { "type": "string", "format": "email" },
    "phoneNumber": { "type": "string" },
    "password": { "type": "string" }
  }
}
```

### 3. RESPONSE tipləri və mənası

**Uğur (200):** Ümumi Result zərfi; `model` boş və ya yaradılan məlumat ola bilər.

```json
{
  "httpStatus": 200,
  "messages": [],
  "model": null
}
```

**Uğursuzluq (400):** Validasiya və ya biznes xətaları `messages`-da (məs. email artıq mövcuddur).

### 4. AUTH

- **Rol tələbi:** yoxdur (`[AllowAnonymous]`).
- **Məntiq:** Yalnız adi (client) istifadəçi qeydiyyatı; həkim deyil.

### 5. ƏLAQƏLİ API-lar

- [POST api/Login](Login.md#post-traditional-login) — qeydiyyatdan dərhal sonra giriş.
- [POST api/Doctors/register](Doctors.md#post-registertraditionaldoctorasync) — həkim kimi qeydiyyat.

### 6. Nə zaman istifadə edilməlidir

- Yeni pasient/client hesabı yaratmaq üçün.
- Həkim olmayan istifadəçi qeydiyyatı üçün.

### 7. Nə zaman istifadə etmək olmaz

- Həkim qeydiyyatı üçün — [api/Doctors/register](Doctors.md#post-registertraditionaldoctorasync).
- Artıq qeydiyyatlı istifadəçi üçün — giriş [api/Login](Login.md#post-traditional-login) ilə edilməlidir.

---

## POST — Registration (Google / Facebook / Apple)

**URL:** `POST api/Registration/Google`, `POST api/Registration/Facebook`, `POST api/Registration/Apple`

Hazırda bu endpointlər **400 Bad Request** qaytarır; sosial giriş/qeydiyyat hələ implementasiya olunmayıb. İstifadə etməyin.

---

[← Login](Login.md) | [API indeksinə qayıt](../README.md) | [AdminUsers](AdminUsers.md) | [Doctors](Doctors.md)
