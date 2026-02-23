# Login Controller

Bu səhifə **Login** controllerinin endpointlərini aşağıdakı strukturda təsvir edir: URL, BODY (JSON schema), RESPONSE, AUTH, ƏLAQƏLİ API-lar, Nə zaman istifadə edilməlidir, Nə zaman istifadə etmək olmaz.

---

## POST — Traditional Login

### 1. URL

```
POST api/Login
```

### 2. BODY (JSON Schema)

```json
{
  "type": "object",
  "required": [ "username", "password", "deviceInfo" ],
  "properties": {
    "username": { "type": "string" },
    "password": { "type": "string" },
    "deviceInfo": {
      "type": "object",
      "required": [ "name", "deviceType", "appType", "uuid" ],
      "properties": {
        "name": { "type": "string" },
        "deviceType": { "type": "integer", "description": "0=Unknown, 1=Mobile, 2=Tablet, 3=Desktop, ..." },
        "appType": { "type": "integer", "description": "0=Web, 1=Android, 2=iOS, ..." },
        "osName": { "type": [ "string", "null" ] },
        "osVersion": { "type": [ "string", "null" ] },
        "uuid": { "type": "string" }
      }
    }
  }
}
```

### 3. RESPONSE tipləri və mənası

**Uğur (200):** Body yalnız access token; refresh token cookie-dadır.

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs..."
}
```

- **accessToken** — JWT; sonrakı sorğularda `Authorization: Bearer <accessToken>` göndərin.

**Uğursuzluq (400/401):** Ümumi Result zərfi (bak [README — Response zərfi](../README.md#ümumi-response-zərfi-result--resultt)).

```json
{
  "httpStatus": 401,
  "messages": [ { "textCode": "ERR...", "text": "...", "exception": null } ],
  "model": null
}
```

### 4. AUTH

- **Rol tələbi:** yoxdur (`[AllowAnonymous]`).
- **Məntiq:** Token tələb olunmur; istifadəçi adı və parol ilə giriş üçündür.

### 5. ƏLAQƏLİ API-lar

- [POST api/Login/refresh](#post-refresh-token) — access token bitəndə yeniləmək.
- [POST api/Registration](Registration.md#post-traditional-client-registration) — yeni hesab yaratmaq.
- [POST api/Doctors/register](Doctors.md#post-registertraditionaldoctorasync) — həkim qeydiyyatı.

### 6. Nə zaman istifadə edilməlidir

- İstifadəçi email/parol ilə daxil olmaq istəyəndə.
- Qeydiyyatdan sonra ilk girişdə.
- Access token vaxtı bitəndə əvəzinə əvvəlcə [Refresh](#post-refresh-token) istifadə edin; uğursuzdursa bu endpoint ilə yenidən giriş edin.

### 7. Nə zaman istifadə etmək olmaz

- Artıq etibarlı access token varsa (API sorğuları üçün token kifayətdir).
- Token yeniləmək üçün — [Refresh](#post-refresh-token) istifadə edin.
- Həkim qeydiyyatı üçün — [api/Doctors/register](Doctors.md#post-registertraditionaldoctorasync) istifadə edin.

---

## POST — Refresh Token

### 1. URL

```
POST api/Login/refresh
```

### 2. BODY (JSON Schema)

Body optional; göndərilməsə `RefreshToken` cookie-dan oxunur.

```json
{
  "type": "object",
  "properties": {
    "refreshToken": { "type": "string" }
  }
}
```

### 3. RESPONSE tipləri və mənası

**Uğur (200):** Yeni access token body-də; refresh token yenilənə bilər (cookie).

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Uğursuzluq (401 və s.):** Ümumi Result zərfi; `messages`-da səbəb göstərilir.

### 4. AUTH

- **Rol tələbi:** yoxdur (`[AllowAnonymous]`).
- **Məntiq:** Refresh token ya body-də ya cookie-də (`RefreshToken`) göndərilir; etibarlı olmalıdır.

### 5. ƏLAQƏLİ API-lar

- [POST api/Login](#post-traditional-login) — ilk giriş və ya refresh uğursuz olanda yenidən giriş.

### 6. Nə zaman istifadə edilməlidir

- Access token vaxtı bitəndə və ya bitməyə yaxın olanda.
- Refresh token hələ etibarlı olanda (yenidən parol yazmadan sessiya uzatmaq).

### 7. Nə zaman istifadə etmək olmaz

- İlk dəfə giriş üçün — [POST api/Login](#post-traditional-login).
- Refresh token ləğv edilibsə və ya etibarsızdırsa — yenidən [Login](#post-traditional-login) edin.

---

[← API indeksinə qayıt](../README.md) | [Registration →](Registration.md) | [AdminUsers](AdminUsers.md) | [Doctors](Doctors.md)
