# Validation Error Codes (TraditionalUserRegisterValidation)

This document lists the validation error codes and their default (English) messages used in `TraditionalUserRegisterValidation`.

> ✅ Tip: Treat **Error Code** as a stable identifier for UI and localization.  
> ✅ Message text can be localized per language without changing the code.

---

## Localization Notes (Recommended Approach)

### 1) Use Error Code as Localization Key
Instead of localizing raw English text, localize by **Error Code** (stable key).
- UI / API clients can map `ERR00014` → localized string
- Back-end can also translate messages before returning response (optional)

### 2) Response Shape Recommendation
Return validation errors like:

```json
{
  "errors": [
    { "field": "PhoneNumber", "code": "ERR00015", "message": "Phone number must be in international E.164 format (e.g., +994501234567)." }
  ]
}
```

---

## Error Codes Table

| Code  | Message | Description |
|----------|---------------|-------------|
| ERR00001 | Name is required. | `Name` field must not be empty. |
| ERR00002 | Name must not exceed 50 characters. | `Name` length must be **<= 50**. |
| ERR00003 | Name can contain letters only. | `Name` must contain **letters only** (no digits/symbols). |
| ERR00004 | Surname is required. | `Surname` field must not be empty. |
| ERR00005 | Surname must not exceed 50 characters. | `Surname` length must be **<= 50**. |
| ERR00006 | Surname can contain letters only. | `Surname` must contain **letters only** (no digits/symbols). |
| ERR00007 | Father name is required. | `FatherName` field must not be empty. |
| ERR00008 | Father name must not exceed 50 characters. | `FatherName` length must be **<= 50**. |
| ERR00009 | Father name can contain letters only. | `FatherName` must contain **letters only** (no digits/symbols). |
| ERR00010 | Birth date is required. | `BirthDate` must be provided (not default/empty). |
| ERR00011 | Birth date cannot be in the future. | `BirthDate` must be **less than today**. |
| ERR00012 | Email is required. | `Email` field must not be empty. |
| ERR00013 | Email format is invalid. | `Email` must match a valid email format. |
| ERR00014 | Phone number is required. | `PhoneNumber` field must not be empty. |
| ERR00015 | Phone number must be in international E.164 format (e.g., +994501234567). | `PhoneNumber` must match **E.164** format. |
| ERR00016 | Password is required. | `Password` field must not be empty. |
| ERR00017 | Password must be at least 8 characters long. | `Password` length must be **>= 8**. |
| ERR00018 | Password must contain at least one uppercase letter. | `Password` must contain **>= 1 uppercase** character. |
| ERR00019 | Password must contain at least one lowercase letter. | `Password` must contain **>= 1 lowercase** character. |
| ERR00020 | Password must contain at least one digit. | `Password` must contain **>= 1 digit**. |
| ERR00021 | Password must contain at least one special character. | `Password` must contain **>= 1 special** character (symbol). |
| ERR00022 | Email Already Exist!||
| ERR00023 | Phone Number Already Exist!||
| ERR00024 | User does not exist||
| ERR00025 | Password is incorrect |
| ERR00026 | User found. But used another provider.|
| ERR00027 | Device name is required. |
| ERR00028 | Device name must not exceed 150 characters.|
| ERR00029 | Invalid device type.|
| ERR00030 | Invalid application type.|
| ERR00031 | OS name must not exceed 150 characters.|
| ERR00032 | OS version must not exceed 150 characters.|
| ERR00033 | UUID is required.|
| ERR00034 | UUID must not exceed 300 characters. |
| ERR00035 | Username is required.|
| ERR00036 | Username must not exceed 300 characters.|
| ERR00037 | Device information is required.|
| ERR00038 | Refresh token is required. |
| ERR00039 | Refresh token must not exceed 512 characters. |
| ERR00040 | Name is required. | Classifier name must not be empty. |
| ERR00041 | Name must not exceed 150 characters. | Classifier name length must be **<= 150**. |
| ERR00042 | Name contains invalid characters. | Classifier name can contain letters, digits, spaces, and `-`, `_`, `.`. |
| ERR00043 | Description is required. | Classifier description must not be empty. |
| ERR00044 | Description must not exceed 500 characters. | Classifier description length must be **<= 500**. |
| ERR00045 | Description contains invalid characters. | Classifier description can contain letters, digits, spaces, and `-`, `_`, `.`, `,`, `:`, `;`, `(`, `)`, `/`, `&`, `+`, `'`. |
| ERR00046 | Coefficient must be greater than 0. | Currency coefficient must be positive. |
| ERR00047 | Coefficient must not exceed 999999.99. | Currency coefficient must not exceed **999999.99**. |
| ERR00048 | Period time must be between 1 and 255 minutes. | Period time must be between **1** and **255** minutes. |
| ERR00050 | Classifier item not found. | The classifier record does not exist. |
| ERR00051 | Classifier name already exists. | Duplicate classifier name conflict. |
| ERR00052 | Refresh token must not contain whitespace. |
| ERR00053 | Refresh token format is invalid. |
| ERR00054 | Refresh token is invalid or expired. |
| ERR00055 | Doctor registered |
| ERR00056 | Doctor cannot found |
| ERR00057 | Doctor specalty cannot found |
| ERR00058 | Doctor is not confirmed yet. Doctor cannot confirm specialty before Doctor Confirm|
| ERR00100 | Unexpected error contact with admin | |
| ERR00101 | Invalid object type | |
| ERR00102 | Page number must be greater than 0. | |
| ERR00103 | Page size must be greater than 0. | |
| ERR00104 | Page size must not exceed 100. | |
| ERR00105 | Weekly schema name is required. | `Name` field must not be empty. |
| ERR00106 | Weekly schema name must not exceed 100 characters. | `Name` length must be **<= 100**. |
| ERR00107 | Weekly schema ColorHex is required. | `ColorHex` field must not be empty. |
| ERR00108 | Weekly schema ColorHex must be 9 characters in format #RRGGBBAA. | RGBA hex format, 9 chars. |
| ERR00109 | DoctorId must be greater than 0. | `DoctorId` required for weekly schema create. |
| ERR00110 | Day schema WeeklySchemaId must be greater than 0. | `WeeklySchemaId` required for day schema create. |
| ERR00111 | Day schema SpecialtyId must be greater than 0. | `SpecialtyId` required. |
| ERR00112 | Day schema PeriodId must be greater than 0. | `PeriodId` required. |
| ERR00113 | Day schema DayOfWeek must be between 1 (Monday) and 7 (Sunday). | Valid weekday. |
| ERR00114 | Day schema OpenTime is required. | Reserved for OpenTime validation. |
| ERR00115 | Day break StartTime is required. | Reserved for StartTime validation. |
| ERR00116 | Day break EndTime is required. | Reserved for EndTime validation. |
| ERR00117 | Day break EndTime must be after StartTime. | Break interval must be valid. |
| ERR00118 | Day break name must not exceed 150 characters. | `Name` length must be **<= 150** when provided. |
| ERR00119 | Day schema PlanPaddingTypeId must be greater than 0 when provided. | When not null, must be positive. |
| ERR00120 | Day break DaySchemaId must be greater than 0. | `DaySchemaId` required for day break. |
| ERR00121 | Weekly schema id must be greater than 0. | Required for create day plans from schema. |
| ERR00122 | Start date is required. | Start date for the target week is required. |
| ERR00123 | Currency id must be greater than 0. | Required for period plan price. |
| ERR00124 | Price per period must be greater than or equal to 0. | Price per period cannot be negative. |
| ERR00125 | Only verified doctors can create day plans from schema. | Doctor must be confirmed. |
| ERR00126 | Weekly schema (template) not found. | Template with given id does not exist. |
| ERR00127 | Weekly schema must have exactly 7 day schemas (one per weekday). | Invalid week data; 6 or 8 days is invalid. |
| ERR00128 | Period or break time overlap detected. Invalid schedule data. | DayBreak and Period times must not overlap. |
| ERR00129 | Invalid padding position. Must be a valid PlanPaddingPosition enum value (0-4). | PlanPaddingPosition value is not defined in enum. |
| ERR00130 | Day schema PeriodCount must be between 0 and 255. | Number of periods per day must be in valid range. |
| ERR00131 | Weekly schema (template) is required. | CreateDayPlansFromSchemaDto.WeeklySchema must not be null. |
| ERR00132 | Day schema PeriodTimeMinutes must be between 1 and 255 when day is not closed. | Period duration in minutes when day has slots. |
