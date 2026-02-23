# AdminUsers Controller

Hər endpoint aşağıdakı ardıcıllıqla təsvir olunur: 1. URL · 2. BODY (JSON schema) · 3. RESPONSE · 4. AUTH · 5. ƏLAQƏLİ API-lar · 6. Nə zaman istifadə edilməlidir · 7. Nə zaman istifadə etmək olmaz.

---

## GET — GetUsersAsync (AdminUsers siyahısı)

### 1. URL

```
GET api/AdminUsers
```

### 2. BODY

GET sorğusu üçün body yoxdur. Parametrlər **query string** ilə göndərilir.

**Query (JSON schema ekvivalentı):**

```json
{
  "pageNumber": 1,
  "pageSize": 20,
  "nameFilter": "string | null",
  "surnameFilter": "string | null",
  "emailFilter": "string | null",
  "phoneFilter": "string | null",
  "userTypeFilter": 0
}
```

`userTypeFilter`: 0 = User, 1 = Doctor, 2 = OrganizationAdmin, 3 = SystemAdmin.

### 3. RESPONSE tipləri və mənası

**Uğur (200):** `model` — səhifələnmiş istifadəçi siyahısı.

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
        "fatherName": "string",
        "phoneNumber": "string",
        "email": "string",
        "userTypes": [ 0, 1 ]
      }
    ],
    "totalCount": 100,
    "totalPages": 5,
    "pageNumber": 1,
    "pageSize": 20
  }
}
```

**Uğursuzluq (403 və s.):** Ümumi Result zərfi; admin olmayanlar üçün 403.

### 4. AUTH

- **Rol:** yalnız **SystemAdmin**.
- **Məntiq:** Bütün istifadəçilərin siyahısını görmək üçün admin hüququ tələb olunur.

### 5. ƏLAQƏLİ API-lar

- [GET api/AdminUsers/{userId}](#get-getuserbyidasync-adminusers-detallı) — bir istifadəçinin detallı məlumatı.
- [GET api/Doctors](Doctors.md#get-getallasync-doctors-siyahısı) — yalnız həkimlərin siyahısı.
- [DELETE api/AdminUsers/{userId}](#delete-deleteuserasync) — istifadəçi silmək.

### 6. Nə zaman istifadə edilməlidir

- Admin paneldə istifadəçi siyahısını səhifələmək və filtrləmək üçün.
- Ad, email, telefon, rol üzrə axtarış üçün.

### 7. Nə zaman istifadə etmək olmaz

- Rol SystemAdmin deyilsə.
- Tək bir istifadəçi məlumatı lazımdırsa — [GET api/AdminUsers/{userId}](#get-getuserbyidasync-adminusers-detallı) istifadə edin.

---

## GET — GetUserByIdAsync (AdminUsers detallı)

### 1. URL

```
GET api/AdminUsers/{userId}
```

`userId`: long (route parametri).

### 2. BODY

Yoxdur (GET).

### 3. RESPONSE tipləri və mənası

**Uğur (200):** `model` — istifadəçi detalları.

```json
{
  "httpStatus": 200,
  "messages": [],
  "model": {
    "id": 1,
    "provider": 0,
    "name": "string",
    "surname": "string",
    "fatherName": "string",
    "email": "string",
    "phoneNumber": "string",
    "birthDate": "2020-01-01T00:00:00",
    "imagePath": "string | null",
    "userTypes": [ 0, 1 ]
  }
}
```

**404:** İstifadəçi tapılmadı. **403:** Rol SystemAdmin deyil.

### 4. AUTH

- **Rol:** **SystemAdmin**.
- **Məntiq:** İstifadəçi ID-si route-dadır; yalnız admin görə bilər.

### 5. ƏLAQƏLİ API-lar

- [GET api/AdminUsers](#get-getusersasync-adminusers-siyahısı) — siyahıdan ID götürmək üçün.
- [DELETE api/AdminUsers/{userId}](#delete-deleteuserasync) — bu istifadəçini silmək.

### 6. Nə zaman istifadə edilməlidir

- Siyahıdan seçilmiş istifadəçinin tam məlumatını göstərmək üçün.
- Silmə və ya redaktə əvvəli məlumatı yoxlamaq üçün.

### 7. Nə zaman istifadə etmək olmaz

- SystemAdmin deyilsə.
- Siyahı lazımdırsa — [GET api/AdminUsers](#get-getusersasync-adminusers-siyahısı).

---

## DELETE — DeleteUserAsync

### 1. URL

```
DELETE api/AdminUsers/{userId}
```

### 2. BODY

Yoxdur.

### 3. RESPONSE tipləri və mənası

**Uğur (200):** Ümumi Result zərfi; `model` boş ola bilər.

**404:** İstifadəçi tapılmadı. **403:** SystemAdmin deyil. **400:** Biznes qaydası (məs. silməyə icazə yoxdur).

### 4. AUTH

- **Rol:** **SystemAdmin**.
- **Məntiq:** İstifadəçi silmək yalnız admin üçündür.

### 5. ƏLAQƏLİ API-lar

- [GET api/AdminUsers/{userId}](#get-getuserbyidasync-adminusers-detallı) — silməzdən əvvəl məlumatı görmək.
- [GET api/AdminUsers](#get-getusersasync-adminusers-siyahısı) — silmədən sonra siyahını yeniləmək.

### 6. Nə zaman istifadə edilməlidir

- Admin istifadəçini sistemdən çıxartmaq istəyəndə.
- Müvafiq biznes qaydalarına uyğun silmə üçün.

### 7. Nə zaman istifadə etmək olmaz

- Rol SystemAdmin deyilsə.
- Öz hesabını silmək üçün (əgər UI icazə vermirsə) — ayrı "hesabı sil" mexaniki ola bilər.

---

[← Login](Login.md) | [Registration](Registration.md) | [API indeksi](../README.md) | [Doctors →](Doctors.md)
