-- ================================================================
-- MedAppointment - Mock Data Seed Script
-- Generated: 2026-04-09
-- ================================================================
-- INSERT ORDER (respecting FK constraints):
--  1. Localization.Languages
--  2. File.Images
--  3. Localization.Resources
--  4. Localization.Translations
--  5. Classifier.Genders
--  6. Classifier.Currencies
--  7. Classifier.PaymentTypes
--  8. Classifier.Periods
--  9. Classifier.PlanPaddingTypes
-- 10. Classifier.Professions
-- 11. Classifier.Specialties
-- 12. Client.People
-- 13. Client.Users
-- 14. Client.Admins
-- 15. Client.Doctors
-- 16. Client.Organizations
-- 17. Security.Devices
-- 18. Security.TraditionalUsers
-- 19. Security.Sessions
-- 20. Security.Tokens
-- 21. Compositions.DoctorSpecialties
-- 22. Compositions.DoctorServiceGenderTypes
-- 23. Compositions.DoctorServiceLanguages
-- 24. Composition.OrganizationUsers
-- 25. Doctor.WeeklySchemas
-- 26. Compositions.WeeklySchemaSpecialties
-- 27. Doctor.DaySchemas
-- 28. Doctor.DayBreaks
-- 29. Payment.Payments
-- 30. Service.DayPlans
-- 31. Compositions.DayPlanSpecialties
-- 32. Service.PeriodPlans
-- 33. Service.Appointments
-- 34. Communication.Chats
-- 35. Communication.ChatHistories
-- 36. Communication.Meets
-- ================================================================

SET NOCOUNT ON;
BEGIN TRANSACTION;

-- ================================================================
-- 1. Localization.Languages  (3 rows)
-- ================================================================
SET IDENTITY_INSERT [Classifier].[Languages] ON;
INSERT INTO [Classifier].[Languages] (Id, [Name], IsDefault, CreatedAt, IsDeleted) VALUES
(1, N'Azərbaycan dili', 1, GETUTCDATE(), 0),
(2, N'English',         0, GETUTCDATE(), 0),
(3, N'Русский',         0, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Classifier].[Languages] OFF;

-- ================================================================
-- 2. File.Images  (12 rows)
-- ================================================================
SET IDENTITY_INSERT [File].[Images] ON;
INSERT INTO [File].[Images] (Id, [Filename], MimeType, FilePath, CreatedAt, IsDeleted) VALUES
( 1, 'ali_hasanov.jpg',       'image/jpeg', '/uploads/profiles/ali_hasanov.jpg',       GETUTCDATE(), 0),
( 2, 'aysel_mammadova.jpg',   'image/jpeg', '/uploads/profiles/aysel_mammadova.jpg',   GETUTCDATE(), 0),
( 3, 'tural_aliyev.jpg',      'image/jpeg', '/uploads/profiles/tural_aliyev.jpg',      GETUTCDATE(), 0),
( 4, 'nigar_huseynova.jpg',   'image/jpeg', '/uploads/profiles/nigar_huseynova.jpg',   GETUTCDATE(), 0),
( 5, 'rashad_guliyev.jpg',    'image/jpeg', '/uploads/profiles/rashad_guliyev.jpg',     GETUTCDATE(), 0),
( 6, 'laman_babayeva.jpg',    'image/jpeg', '/uploads/profiles/laman_babayeva.jpg',     GETUTCDATE(), 0),
( 7, 'orkhan_nasirov.jpg',    'image/jpeg', '/uploads/profiles/orkhan_nasirov.jpg',     GETUTCDATE(), 0),
( 8, 'gunel_safarova.jpg',    'image/jpeg', '/uploads/profiles/gunel_safarova.jpg',     GETUTCDATE(), 0),
( 9, 'kamran_ismayilov.jpg',  'image/jpeg', '/uploads/profiles/kamran_ismayilov.jpg',   GETUTCDATE(), 0),
(10, 'aynur_rzayeva.jpg',     'image/jpeg', '/uploads/profiles/aynur_rzayeva.jpg',      GETUTCDATE(), 0),
(11, 'samir_valiyev.jpg',     'image/jpeg', '/uploads/profiles/samir_valiyev.jpg',      GETUTCDATE(), 0),
(12, 'leyla_ahmadova.jpg',    'image/jpeg', '/uploads/profiles/leyla_ahmadova.jpg',     GETUTCDATE(), 0);
SET IDENTITY_INSERT [File].[Images] OFF;

-- ================================================================
-- 3. Localization.Resources  (70 rows)
-- ================================================================
-- Resource mapping:
--   1-6   : Genders (name+desc x3)
--   7-12  : Currencies (name+desc x3)
--   13-18 : PaymentTypes (name+desc x3)
--   19-26 : Periods (name+desc x4)
--   27-34 : PlanPaddingTypes (name+desc x4)
--   35-44 : Professions (name+desc x5)
--   45-60 : Specialties (name+desc x8)
--   61-70 : Doctor titles+descriptions (title+desc x5)
-- ================================================================
SET IDENTITY_INSERT [Localization].[Resources] ON;
INSERT INTO [Localization].[Resources] (Id, [Key], CreatedAt, IsDeleted) VALUES
-- Genders
( 1, 'classifier.gender.male.name',        GETUTCDATE(), 0),
( 2, 'classifier.gender.male.desc',        GETUTCDATE(), 0),
( 3, 'classifier.gender.female.name',      GETUTCDATE(), 0),
( 4, 'classifier.gender.female.desc',      GETUTCDATE(), 0),
( 5, 'classifier.gender.other.name',       GETUTCDATE(), 0),
( 6, 'classifier.gender.other.desc',       GETUTCDATE(), 0),
-- Currencies
( 7, 'classifier.currency.azn.name',       GETUTCDATE(), 0),
( 8, 'classifier.currency.azn.desc',       GETUTCDATE(), 0),
( 9, 'classifier.currency.usd.name',       GETUTCDATE(), 0),
(10, 'classifier.currency.usd.desc',       GETUTCDATE(), 0),
(11, 'classifier.currency.eur.name',       GETUTCDATE(), 0),
(12, 'classifier.currency.eur.desc',       GETUTCDATE(), 0),
-- PaymentTypes
(13, 'classifier.paymenttype.cash.name',   GETUTCDATE(), 0),
(14, 'classifier.paymenttype.cash.desc',   GETUTCDATE(), 0),
(15, 'classifier.paymenttype.card.name',   GETUTCDATE(), 0),
(16, 'classifier.paymenttype.card.desc',   GETUTCDATE(), 0),
(17, 'classifier.paymenttype.online.name', GETUTCDATE(), 0),
(18, 'classifier.paymenttype.online.desc', GETUTCDATE(), 0),
-- Periods
(19, 'classifier.period.15min.name',       GETUTCDATE(), 0),
(20, 'classifier.period.15min.desc',       GETUTCDATE(), 0),
(21, 'classifier.period.30min.name',       GETUTCDATE(), 0),
(22, 'classifier.period.30min.desc',       GETUTCDATE(), 0),
(23, 'classifier.period.45min.name',       GETUTCDATE(), 0),
(24, 'classifier.period.45min.desc',       GETUTCDATE(), 0),
(25, 'classifier.period.60min.name',       GETUTCDATE(), 0),
(26, 'classifier.period.60min.desc',       GETUTCDATE(), 0),
-- PlanPaddingTypes
(27, 'classifier.padding.start.name',      GETUTCDATE(), 0),
(28, 'classifier.padding.start.desc',      GETUTCDATE(), 0),
(29, 'classifier.padding.end.name',        GETUTCDATE(), 0),
(30, 'classifier.padding.end.desc',        GETUTCDATE(), 0),
(31, 'classifier.padding.linear.name',     GETUTCDATE(), 0),
(32, 'classifier.padding.linear.desc',     GETUTCDATE(), 0),
(33, 'classifier.padding.center.name',     GETUTCDATE(), 0),
(34, 'classifier.padding.center.desc',     GETUTCDATE(), 0),
-- Professions
(35, 'classifier.profession.gp.name',              GETUTCDATE(), 0),
(36, 'classifier.profession.gp.desc',              GETUTCDATE(), 0),
(37, 'classifier.profession.surgeon.name',          GETUTCDATE(), 0),
(38, 'classifier.profession.surgeon.desc',          GETUTCDATE(), 0),
(39, 'classifier.profession.cardiologist.name',     GETUTCDATE(), 0),
(40, 'classifier.profession.cardiologist.desc',     GETUTCDATE(), 0),
(41, 'classifier.profession.dermatologist.name',    GETUTCDATE(), 0),
(42, 'classifier.profession.dermatologist.desc',    GETUTCDATE(), 0),
(43, 'classifier.profession.pediatrician.name',     GETUTCDATE(), 0),
(44, 'classifier.profession.pediatrician.desc',     GETUTCDATE(), 0),
-- Specialties
(45, 'classifier.specialty.internal.name',          GETUTCDATE(), 0),
(46, 'classifier.specialty.internal.desc',          GETUTCDATE(), 0),
(47, 'classifier.specialty.surgery.name',           GETUTCDATE(), 0),
(48, 'classifier.specialty.surgery.desc',           GETUTCDATE(), 0),
(49, 'classifier.specialty.cardiology.name',        GETUTCDATE(), 0),
(50, 'classifier.specialty.cardiology.desc',        GETUTCDATE(), 0),
(51, 'classifier.specialty.dermatology.name',       GETUTCDATE(), 0),
(52, 'classifier.specialty.dermatology.desc',       GETUTCDATE(), 0),
(53, 'classifier.specialty.pediatrics.name',        GETUTCDATE(), 0),
(54, 'classifier.specialty.pediatrics.desc',        GETUTCDATE(), 0),
(55, 'classifier.specialty.neurology.name',         GETUTCDATE(), 0),
(56, 'classifier.specialty.neurology.desc',         GETUTCDATE(), 0),
(57, 'classifier.specialty.orthopedics.name',       GETUTCDATE(), 0),
(58, 'classifier.specialty.orthopedics.desc',       GETUTCDATE(), 0),
(59, 'classifier.specialty.ophthalmology.name',     GETUTCDATE(), 0),
(60, 'classifier.specialty.ophthalmology.desc',     GETUTCDATE(), 0),
-- Doctor Titles & Descriptions
(61, 'doctor.1.title',  GETUTCDATE(), 0),
(62, 'doctor.1.desc',   GETUTCDATE(), 0),
(63, 'doctor.2.title',  GETUTCDATE(), 0),
(64, 'doctor.2.desc',   GETUTCDATE(), 0),
(65, 'doctor.3.title',  GETUTCDATE(), 0),
(66, 'doctor.3.desc',   GETUTCDATE(), 0),
(67, 'doctor.4.title',  GETUTCDATE(), 0),
(68, 'doctor.4.desc',   GETUTCDATE(), 0),
(69, 'doctor.5.title',  GETUTCDATE(), 0),
(70, 'doctor.5.desc',   GETUTCDATE(), 0);
SET IDENTITY_INSERT [Localization].[Resources] OFF;

-- ================================================================
-- 4. Localization.Translations  (210 rows = 70 resources x 3 langs)
-- ================================================================
-- LanguageId: 1=AZ, 2=EN, 3=RU
-- ================================================================
SET IDENTITY_INSERT [Localization].[Translations] ON;
INSERT INTO [Localization].[Translations] (Id, ResourceId, LanguageId, [Text], CreatedAt, IsDeleted) VALUES
-- Gender: Male
(  1,  1, 1, N'Kişi',                    GETUTCDATE(), 0),
(  2,  1, 2, N'Male',                    GETUTCDATE(), 0),
(  3,  1, 3, N'Мужской',                 GETUTCDATE(), 0),
(  4,  2, 1, N'Kişi cinsi',              GETUTCDATE(), 0),
(  5,  2, 2, N'Male gender',             GETUTCDATE(), 0),
(  6,  2, 3, N'Мужской пол',             GETUTCDATE(), 0),
-- Gender: Female
(  7,  3, 1, N'Qadın',                   GETUTCDATE(), 0),
(  8,  3, 2, N'Female',                  GETUTCDATE(), 0),
(  9,  3, 3, N'Женский',                 GETUTCDATE(), 0),
( 10,  4, 1, N'Qadın cinsi',             GETUTCDATE(), 0),
( 11,  4, 2, N'Female gender',           GETUTCDATE(), 0),
( 12,  4, 3, N'Женский пол',             GETUTCDATE(), 0),
-- Gender: Other
( 13,  5, 1, N'Digər',                   GETUTCDATE(), 0),
( 14,  5, 2, N'Other',                   GETUTCDATE(), 0),
( 15,  5, 3, N'Другой',                  GETUTCDATE(), 0),
( 16,  6, 1, N'Digər cins',              GETUTCDATE(), 0),
( 17,  6, 2, N'Other gender',            GETUTCDATE(), 0),
( 18,  6, 3, N'Другой пол',              GETUTCDATE(), 0),
-- Currency: AZN
( 19,  7, 1, N'Azərbaycan Manatı',       GETUTCDATE(), 0),
( 20,  7, 2, N'Azerbaijani Manat',       GETUTCDATE(), 0),
( 21,  7, 3, N'Азербайджанский манат',   GETUTCDATE(), 0),
( 22,  8, 1, N'AZN valyutası',           GETUTCDATE(), 0),
( 23,  8, 2, N'AZN currency',            GETUTCDATE(), 0),
( 24,  8, 3, N'Валюта AZN',              GETUTCDATE(), 0),
-- Currency: USD
( 25,  9, 1, N'ABŞ Dolları',             GETUTCDATE(), 0),
( 26,  9, 2, N'US Dollar',               GETUTCDATE(), 0),
( 27,  9, 3, N'Доллар США',              GETUTCDATE(), 0),
( 28, 10, 1, N'USD valyutası',           GETUTCDATE(), 0),
( 29, 10, 2, N'USD currency',            GETUTCDATE(), 0),
( 30, 10, 3, N'Валюта USD',              GETUTCDATE(), 0),
-- Currency: EUR
( 31, 11, 1, N'Avro',                    GETUTCDATE(), 0),
( 32, 11, 2, N'Euro',                    GETUTCDATE(), 0),
( 33, 11, 3, N'Евро',                    GETUTCDATE(), 0),
( 34, 12, 1, N'EUR valyutası',           GETUTCDATE(), 0),
( 35, 12, 2, N'EUR currency',            GETUTCDATE(), 0),
( 36, 12, 3, N'Валюта EUR',              GETUTCDATE(), 0),
-- PaymentType: Cash
( 37, 13, 1, N'Nağd',                    GETUTCDATE(), 0),
( 38, 13, 2, N'Cash',                    GETUTCDATE(), 0),
( 39, 13, 3, N'Наличные',                GETUTCDATE(), 0),
( 40, 14, 1, N'Nağd ödəniş',             GETUTCDATE(), 0),
( 41, 14, 2, N'Cash payment',            GETUTCDATE(), 0),
( 42, 14, 3, N'Оплата наличными',        GETUTCDATE(), 0),
-- PaymentType: Card
( 43, 15, 1, N'Kart',                    GETUTCDATE(), 0),
( 44, 15, 2, N'Card',                    GETUTCDATE(), 0),
( 45, 15, 3, N'Карта',                   GETUTCDATE(), 0),
( 46, 16, 1, N'Kartla ödəniş',           GETUTCDATE(), 0),
( 47, 16, 2, N'Card payment',            GETUTCDATE(), 0),
( 48, 16, 3, N'Оплата картой',           GETUTCDATE(), 0),
-- PaymentType: Online
( 49, 17, 1, N'Onlayn',                  GETUTCDATE(), 0),
( 50, 17, 2, N'Online',                  GETUTCDATE(), 0),
( 51, 17, 3, N'Онлайн',                  GETUTCDATE(), 0),
( 52, 18, 1, N'Onlayn ödəniş',           GETUTCDATE(), 0),
( 53, 18, 2, N'Online payment',          GETUTCDATE(), 0),
( 54, 18, 3, N'Онлайн-оплата',           GETUTCDATE(), 0),
-- Period: 15 min
( 55, 19, 1, N'15 dəqiqə',              GETUTCDATE(), 0),
( 56, 19, 2, N'15 minutes',              GETUTCDATE(), 0),
( 57, 19, 3, N'15 минут',                GETUTCDATE(), 0),
( 58, 20, 1, N'15 dəqiqəlik period',     GETUTCDATE(), 0),
( 59, 20, 2, N'15-minute period',        GETUTCDATE(), 0),
( 60, 20, 3, N'15-минутный период',      GETUTCDATE(), 0),
-- Period: 30 min
( 61, 21, 1, N'30 dəqiqə',              GETUTCDATE(), 0),
( 62, 21, 2, N'30 minutes',              GETUTCDATE(), 0),
( 63, 21, 3, N'30 минут',                GETUTCDATE(), 0),
( 64, 22, 1, N'30 dəqiqəlik period',     GETUTCDATE(), 0),
( 65, 22, 2, N'30-minute period',        GETUTCDATE(), 0),
( 66, 22, 3, N'30-минутный период',      GETUTCDATE(), 0),
-- Period: 45 min
( 67, 23, 1, N'45 dəqiqə',              GETUTCDATE(), 0),
( 68, 23, 2, N'45 minutes',              GETUTCDATE(), 0),
( 69, 23, 3, N'45 минут',                GETUTCDATE(), 0),
( 70, 24, 1, N'45 dəqiqəlik period',     GETUTCDATE(), 0),
( 71, 24, 2, N'45-minute period',        GETUTCDATE(), 0),
( 72, 24, 3, N'45-минутный период',      GETUTCDATE(), 0),
-- Period: 60 min
( 73, 25, 1, N'60 dəqiqə',              GETUTCDATE(), 0),
( 74, 25, 2, N'60 minutes',              GETUTCDATE(), 0),
( 75, 25, 3, N'60 минут',                GETUTCDATE(), 0),
( 76, 26, 1, N'60 dəqiqəlik period',     GETUTCDATE(), 0),
( 77, 26, 2, N'60-minute period',        GETUTCDATE(), 0),
( 78, 26, 3, N'60-минутный период',      GETUTCDATE(), 0),
-- PlanPaddingType: Start
( 79, 27, 1, N'Başlanğıc',               GETUTCDATE(), 0),
( 80, 27, 2, N'Start',                   GETUTCDATE(), 0),
( 81, 27, 3, N'Начало',                  GETUTCDATE(), 0),
( 82, 28, 1, N'Period başlanğıcında əlavə vaxt',              GETUTCDATE(), 0),
( 83, 28, 2, N'Extra time at start of period',                GETUTCDATE(), 0),
( 84, 28, 3, N'Дополнительное время в начале периода',        GETUTCDATE(), 0),
-- PlanPaddingType: End
( 85, 29, 1, N'Son',                     GETUTCDATE(), 0),
( 86, 29, 2, N'End',                     GETUTCDATE(), 0),
( 87, 29, 3, N'Конец',                   GETUTCDATE(), 0),
( 88, 30, 1, N'Period sonunda əlavə vaxt',                    GETUTCDATE(), 0),
( 89, 30, 2, N'Extra time at end of period',                  GETUTCDATE(), 0),
( 90, 30, 3, N'Дополнительное время в конце периода',         GETUTCDATE(), 0),
-- PlanPaddingType: Linear
( 91, 31, 1, N'Xətti',                   GETUTCDATE(), 0),
( 92, 31, 2, N'Linear',                  GETUTCDATE(), 0),
( 93, 31, 3, N'Линейный',                GETUTCDATE(), 0),
( 94, 32, 1, N'Periodlar arası bərabər paylama',              GETUTCDATE(), 0),
( 95, 32, 2, N'Equal distribution between periods',           GETUTCDATE(), 0),
( 96, 32, 3, N'Равномерное распределение между периодами',    GETUTCDATE(), 0),
-- PlanPaddingType: Center
( 97, 33, 1, N'Mərkəzi',                 GETUTCDATE(), 0),
( 98, 33, 2, N'Center',                  GETUTCDATE(), 0),
( 99, 33, 3, N'Центральный',             GETUTCDATE(), 0),
(100, 34, 1, N'Periodlar arası mərkəzi paylama',              GETUTCDATE(), 0),
(101, 34, 2, N'Center distribution between periods',          GETUTCDATE(), 0),
(102, 34, 3, N'Центральное распределение между периодами',    GETUTCDATE(), 0),
-- Profession: General Practitioner
(103, 35, 1, N'Ümumi Həkim',             GETUTCDATE(), 0),
(104, 35, 2, N'General Practitioner',    GETUTCDATE(), 0),
(105, 35, 3, N'Врач общей практики',     GETUTCDATE(), 0),
(106, 36, 1, N'Ümumi tibb praktikası mütəxəssisi',            GETUTCDATE(), 0),
(107, 36, 2, N'General medical practice specialist',           GETUTCDATE(), 0),
(108, 36, 3, N'Специалист общей медицинской практики',         GETUTCDATE(), 0),
-- Profession: Surgeon
(109, 37, 1, N'Cərrah',                  GETUTCDATE(), 0),
(110, 37, 2, N'Surgeon',                 GETUTCDATE(), 0),
(111, 37, 3, N'Хирург',                  GETUTCDATE(), 0),
(112, 38, 1, N'Cərrahi əməliyyat mütəxəssisi',               GETUTCDATE(), 0),
(113, 38, 2, N'Surgical operation specialist',                GETUTCDATE(), 0),
(114, 38, 3, N'Специалист по хирургическим операциям',        GETUTCDATE(), 0),
-- Profession: Cardiologist
(115, 39, 1, N'Kardioloq',               GETUTCDATE(), 0),
(116, 39, 2, N'Cardiologist',            GETUTCDATE(), 0),
(117, 39, 3, N'Кардиолог',               GETUTCDATE(), 0),
(118, 40, 1, N'Ürək-damar sistemi mütəxəssisi',              GETUTCDATE(), 0),
(119, 40, 2, N'Cardiovascular system specialist',             GETUTCDATE(), 0),
(120, 40, 3, N'Специалист по сердечно-сосудистой системе',    GETUTCDATE(), 0),
-- Profession: Dermatologist
(121, 41, 1, N'Dermatoloq',              GETUTCDATE(), 0),
(122, 41, 2, N'Dermatologist',           GETUTCDATE(), 0),
(123, 41, 3, N'Дерматолог',              GETUTCDATE(), 0),
(124, 42, 1, N'Dəri xəstəlikləri mütəxəssisi',               GETUTCDATE(), 0),
(125, 42, 2, N'Skin diseases specialist',                     GETUTCDATE(), 0),
(126, 42, 3, N'Специалист по кожным заболеваниям',            GETUTCDATE(), 0),
-- Profession: Pediatrician
(127, 43, 1, N'Pediatr',                 GETUTCDATE(), 0),
(128, 43, 2, N'Pediatrician',            GETUTCDATE(), 0),
(129, 43, 3, N'Педиатр',                 GETUTCDATE(), 0),
(130, 44, 1, N'Uşaq xəstəlikləri mütəxəssisi',               GETUTCDATE(), 0),
(131, 44, 2, N'Children''s diseases specialist',              GETUTCDATE(), 0),
(132, 44, 3, N'Специалист по детским заболеваниям',           GETUTCDATE(), 0),
-- Specialty: Internal Medicine
(133, 45, 1, N'Daxili Xəstəliklər',      GETUTCDATE(), 0),
(134, 45, 2, N'Internal Medicine',       GETUTCDATE(), 0),
(135, 45, 3, N'Внутренние болезни',      GETUTCDATE(), 0),
(136, 46, 1, N'Daxili orqanların müalicəsi',                  GETUTCDATE(), 0),
(137, 46, 2, N'Treatment of internal organs',                 GETUTCDATE(), 0),
(138, 46, 3, N'Лечение внутренних органов',                   GETUTCDATE(), 0),
-- Specialty: General Surgery
(139, 47, 1, N'Ümumi Cərrahiyyə',        GETUTCDATE(), 0),
(140, 47, 2, N'General Surgery',         GETUTCDATE(), 0),
(141, 47, 3, N'Общая хирургия',          GETUTCDATE(), 0),
(142, 48, 1, N'Cərrahi müdaxilə sahəsi',                     GETUTCDATE(), 0),
(143, 48, 2, N'Surgical intervention field',                  GETUTCDATE(), 0),
(144, 48, 3, N'Область хирургического вмешательства',         GETUTCDATE(), 0),
-- Specialty: Cardiology
(145, 49, 1, N'Kardiologiya',            GETUTCDATE(), 0),
(146, 49, 2, N'Cardiology',              GETUTCDATE(), 0),
(147, 49, 3, N'Кардиология',             GETUTCDATE(), 0),
(148, 50, 1, N'Ürək xəstəliklərinin müalicəsi',              GETUTCDATE(), 0),
(149, 50, 2, N'Treatment of heart diseases',                  GETUTCDATE(), 0),
(150, 50, 3, N'Лечение заболеваний сердца',                   GETUTCDATE(), 0),
-- Specialty: Dermatology
(151, 51, 1, N'Dermatologiya',           GETUTCDATE(), 0),
(152, 51, 2, N'Dermatology',             GETUTCDATE(), 0),
(153, 51, 3, N'Дерматология',            GETUTCDATE(), 0),
(154, 52, 1, N'Dəri xəstəliklərinin müalicəsi',              GETUTCDATE(), 0),
(155, 52, 2, N'Treatment of skin diseases',                   GETUTCDATE(), 0),
(156, 52, 3, N'Лечение кожных заболеваний',                   GETUTCDATE(), 0),
-- Specialty: Pediatrics
(157, 53, 1, N'Pediatriya',              GETUTCDATE(), 0),
(158, 53, 2, N'Pediatrics',              GETUTCDATE(), 0),
(159, 53, 3, N'Педиатрия',               GETUTCDATE(), 0),
(160, 54, 1, N'Uşaq sağlamlığı sahəsi',                      GETUTCDATE(), 0),
(161, 54, 2, N'Children''s health field',                     GETUTCDATE(), 0),
(162, 54, 3, N'Область здоровья детей',                       GETUTCDATE(), 0),
-- Specialty: Neurology
(163, 55, 1, N'Nevrologiya',             GETUTCDATE(), 0),
(164, 55, 2, N'Neurology',               GETUTCDATE(), 0),
(165, 55, 3, N'Неврология',              GETUTCDATE(), 0),
(166, 56, 1, N'Sinir sistemi xəstəlikləri',                   GETUTCDATE(), 0),
(167, 56, 2, N'Nervous system diseases',                      GETUTCDATE(), 0),
(168, 56, 3, N'Заболевания нервной системы',                  GETUTCDATE(), 0),
-- Specialty: Orthopedics
(169, 57, 1, N'Ortopediya',              GETUTCDATE(), 0),
(170, 57, 2, N'Orthopedics',             GETUTCDATE(), 0),
(171, 57, 3, N'Ортопедия',               GETUTCDATE(), 0),
(172, 58, 1, N'Sümük və oynaq xəstəlikləri',                  GETUTCDATE(), 0),
(173, 58, 2, N'Bone and joint diseases',                      GETUTCDATE(), 0),
(174, 58, 3, N'Заболевания костей и суставов',                GETUTCDATE(), 0),
-- Specialty: Ophthalmology
(175, 59, 1, N'Oftalmologiya',           GETUTCDATE(), 0),
(176, 59, 2, N'Ophthalmology',           GETUTCDATE(), 0),
(177, 59, 3, N'Офтальмология',           GETUTCDATE(), 0),
(178, 60, 1, N'Göz xəstəliklərinin müalicəsi',               GETUTCDATE(), 0),
(179, 60, 2, N'Treatment of eye diseases',                    GETUTCDATE(), 0),
(180, 60, 3, N'Лечение глазных заболеваний',                  GETUTCDATE(), 0),
-- Doctor 1: Title & Description
(181, 61, 1, N'Baş Həkim',               GETUTCDATE(), 0),
(182, 61, 2, N'Chief Physician',         GETUTCDATE(), 0),
(183, 61, 3, N'Главный врач',            GETUTCDATE(), 0),
(184, 62, 1, N'20 illik təcrübəyə malik ümumi həkim',         GETUTCDATE(), 0),
(185, 62, 2, N'General practitioner with 20 years experience', GETUTCDATE(), 0),
(186, 62, 3, N'Врач общей практики с 20-летним опытом',       GETUTCDATE(), 0),
-- Doctor 2: Title & Description
(187, 63, 1, N'Baş Cərrah',              GETUTCDATE(), 0),
(188, 63, 2, N'Chief Surgeon',           GETUTCDATE(), 0),
(189, 63, 3, N'Главный хирург',          GETUTCDATE(), 0),
(190, 64, 1, N'15 illik təcrübəyə malik cərrah',              GETUTCDATE(), 0),
(191, 64, 2, N'Surgeon with 15 years experience',             GETUTCDATE(), 0),
(192, 64, 3, N'Хирург с 15-летним опытом',                    GETUTCDATE(), 0),
-- Doctor 3: Title & Description
(193, 65, 1, N'Kardioloq Mütəxəssis',    GETUTCDATE(), 0),
(194, 65, 2, N'Cardiology Specialist',   GETUTCDATE(), 0),
(195, 65, 3, N'Специалист-кардиолог',    GETUTCDATE(), 0),
(196, 66, 1, N'Ürək-damar cərrahiyyəsi mütəxəssisi',          GETUTCDATE(), 0),
(197, 66, 2, N'Cardiovascular surgery specialist',            GETUTCDATE(), 0),
(198, 66, 3, N'Специалист по сердечно-сосудистой хирургии',   GETUTCDATE(), 0),
-- Doctor 4: Title & Description
(199, 67, 1, N'Dermatoloq Mütəxəssis',   GETUTCDATE(), 0),
(200, 67, 2, N'Dermatology Specialist',  GETUTCDATE(), 0),
(201, 67, 3, N'Специалист-дерматолог',   GETUTCDATE(), 0),
(202, 68, 1, N'Dəri xəstəlikləri üzrə mütəxəssis',           GETUTCDATE(), 0),
(203, 68, 2, N'Specialist in skin diseases',                  GETUTCDATE(), 0),
(204, 68, 3, N'Специалист по кожным заболеваниям',            GETUTCDATE(), 0),
-- Doctor 5: Title & Description
(205, 69, 1, N'Pediatr Mütəxəssis',      GETUTCDATE(), 0),
(206, 69, 2, N'Pediatrics Specialist',   GETUTCDATE(), 0),
(207, 69, 3, N'Специалист-педиатр',      GETUTCDATE(), 0),
(208, 70, 1, N'Uşaq sağlamlığı mütəxəssisi',                 GETUTCDATE(), 0),
(209, 70, 2, N'Children''s health specialist',                GETUTCDATE(), 0),
(210, 70, 3, N'Специалист по здоровью детей',                 GETUTCDATE(), 0);
SET IDENTITY_INSERT [Localization].[Translations] OFF;

-- ================================================================
-- 5. Classifier.Genders  (3 rows)
-- ================================================================
SET IDENTITY_INSERT [Classifier].[Genders] ON;
INSERT INTO [Classifier].[Genders] (Id, [Key], NameTextId, DescriptionTextId, CreatedAt, IsDeleted) VALUES
(1, 'GENDER_MALE',   1, 2, GETUTCDATE(), 0),
(2, 'GENDER_FEMALE', 3, 4, GETUTCDATE(), 0),
(3, 'GENDER_OTHER',  5, 6, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Classifier].[Genders] OFF;

-- ================================================================
-- 6. Classifier.Currencies  (3 rows)
-- ================================================================
SET IDENTITY_INSERT [Classifier].[Currencies] ON;
INSERT INTO [Classifier].[Currencies] (Id, [Key], NameTextId, DescriptionTextId, Coefficent, CreatedAt, IsDeleted) VALUES
(1, 'CURRENCY_AZN', 7,  8,  1.0000, GETUTCDATE(), 0),
(2, 'CURRENCY_USD', 9,  10, 1.7000, GETUTCDATE(), 0),
(3, 'CURRENCY_EUR', 11, 12, 1.8500, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Classifier].[Currencies] OFF;

-- ================================================================
-- 7. Classifier.PaymentTypes  (3 rows)
-- ================================================================
SET IDENTITY_INSERT [Classifier].[PaymentTypes] ON;
INSERT INTO [Classifier].[PaymentTypes] (Id, [Key], NameTextId, DescriptionTextId, CreatedAt, IsDeleted) VALUES
(1, 'PAYMENT_CASH',   13, 14, GETUTCDATE(), 0),
(2, 'PAYMENT_CARD',   15, 16, GETUTCDATE(), 0),
(3, 'PAYMENT_ONLINE', 17, 18, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Classifier].[PaymentTypes] OFF;

-- ================================================================
-- 8. Classifier.Periods  (4 rows)
-- ================================================================
SET IDENTITY_INSERT [Classifier].[Periods] ON;
INSERT INTO [Classifier].[Periods] (Id, [Key], NameTextId, DescriptionTextId, PeriodTime, CreatedAt, IsDeleted) VALUES
(1, 'PERIOD_15MIN', 19, 20, 15, GETUTCDATE(), 0),
(2, 'PERIOD_30MIN', 21, 22, 30, GETUTCDATE(), 0),
(3, 'PERIOD_45MIN', 23, 24, 45, GETUTCDATE(), 0),
(4, 'PERIOD_60MIN', 25, 26, 60, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Classifier].[Periods] OFF;

-- ================================================================
-- 9. Classifier.PlanPaddingTypes  (4 rows)
-- ================================================================
SET IDENTITY_INSERT [Classifier].[PlanPaddingTypes] ON;
INSERT INTO [Classifier].[PlanPaddingTypes] (Id, [Key], NameTextId, DescriptionTextId, PaddingPosition, PaddingTime, CreatedAt, IsDeleted) VALUES
(1, 'PADDING_START',  27, 28, 1,  5, GETUTCDATE(), 0),
(2, 'PADDING_END',    29, 30, 2,  5, GETUTCDATE(), 0),
(3, 'PADDING_LINEAR', 31, 32, 3, 10, GETUTCDATE(), 0),
(4, 'PADDING_CENTER', 33, 34, 4, 10, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Classifier].[PlanPaddingTypes] OFF;

-- ================================================================
-- 10. Classifier.Professions  (5 rows)
-- ================================================================
SET IDENTITY_INSERT [Classifier].[Professions] ON;
INSERT INTO [Classifier].[Professions] (Id, [Key], NameTextId, DescriptionTextId, CreatedAt, IsDeleted) VALUES
(1, 'PROFESSION_GP',             35, 36, GETUTCDATE(), 0),
(2, 'PROFESSION_SURGEON',        37, 38, GETUTCDATE(), 0),
(3, 'PROFESSION_CARDIOLOGIST',   39, 40, GETUTCDATE(), 0),
(4, 'PROFESSION_DERMATOLOGIST',  41, 42, GETUTCDATE(), 0),
(5, 'PROFESSION_PEDIATRICIAN',   43, 44, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Classifier].[Professions] OFF;

-- ================================================================
-- 11. Classifier.Specialties  (8 rows)
-- ================================================================
SET IDENTITY_INSERT [Classifier].[Specialties] ON;
INSERT INTO [Classifier].[Specialties] (Id, [Key], NameTextId, DescriptionTextId, CreatedAt, IsDeleted) VALUES
(1, 'SPECIALTY_INTERNAL_MEDICINE', 45, 46, GETUTCDATE(), 0),
(2, 'SPECIALTY_GENERAL_SURGERY',   47, 48, GETUTCDATE(), 0),
(3, 'SPECIALTY_CARDIOLOGY',        49, 50, GETUTCDATE(), 0),
(4, 'SPECIALTY_DERMATOLOGY',       51, 52, GETUTCDATE(), 0),
(5, 'SPECIALTY_PEDIATRICS',        53, 54, GETUTCDATE(), 0),
(6, 'SPECIALTY_NEUROLOGY',         55, 56, GETUTCDATE(), 0),
(7, 'SPECIALTY_ORTHOPEDICS',       57, 58, GETUTCDATE(), 0),
(8, 'SPECIALTY_OPHTHALMOLOGY',     59, 60, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Classifier].[Specialties] OFF;

-- ================================================================
-- 12. Client.People  (15 rows)
-- ================================================================
-- People 1-2  : Admins
-- People 3-7  : Doctors
-- People 8-15 : Patients
-- GenderId: 1=Male, 2=Female
-- ================================================================
SET IDENTITY_INSERT [Client].[People] ON;
INSERT INTO [Client].[People] (Id, [Name], Surname, FatherName, ImageId, Email, PhoneNumber, BirthDate, GenderId, CreatedAt, IsDeleted) VALUES
( 1, N'Əli',     N'Həsənov',     N'Fərid',    1,    'ali.hasanov@medapp.az',       '+994501234501', '1985-03-15', 1, GETUTCDATE(), 0),
( 2, N'Aysəl',   N'Məmmədova',   N'Ramiz',    2,    'aysel.mammadova@medapp.az',   '+994502234502', '1990-07-22', 2, GETUTCDATE(), 0),
( 3, N'Tural',   N'Əliyev',      N'Vüqar',    3,    'tural.aliyev@medapp.az',      '+994503234503', '1978-01-10', 1, GETUTCDATE(), 0),
( 4, N'Nigar',   N'Hüseynova',   N'Elçin',    4,    'nigar.huseynova@medapp.az',   '+994504234504', '1982-11-05', 2, GETUTCDATE(), 0),
( 5, N'Rəşad',   N'Quliyev',     N'İlham',    5,    'rashad.guliyev@medapp.az',    '+994505234505', '1975-06-20', 1, GETUTCDATE(), 0),
( 6, N'Ləman',   N'Babayeva',    N'Kamil',    6,    'laman.babayeva@medapp.az',    '+994506234506', '1988-09-12', 2, GETUTCDATE(), 0),
( 7, N'Orxan',   N'Nəsirov',     N'Rauf',     7,    'orkhan.nasirov@medapp.az',    '+994507234507', '1980-04-18', 1, GETUTCDATE(), 0),
( 8, N'Günəl',   N'Səfərova',    N'Mübariz',  8,    'gunel.safarova@gmail.com',    '+994508234508', '1995-02-28', 2, GETUTCDATE(), 0),
( 9, N'Kamran',  N'İsmayılov',   N'Tahir',    9,    'kamran.ismayilov@gmail.com',  '+994509234509', '1992-08-14', 1, GETUTCDATE(), 0),
(10, N'Aynur',   N'Rzayeva',     N'Faiq',     10,   'aynur.rzayeva@gmail.com',     '+994510234510', '1998-12-01', 2, GETUTCDATE(), 0),
(11, N'Samir',   N'Vəliyev',     N'Əhməd',    11,   'samir.valiyev@gmail.com',     '+994511234511', '1993-05-09', 1, GETUTCDATE(), 0),
(12, N'Leyla',   N'Əhmədova',    N'Ceyhun',   12,   'leyla.ahmadova@gmail.com',    '+994512234512', '1997-10-25', 2, GETUTCDATE(), 0),
(13, N'Fərid',   N'Mustafayev',  N'Arif',     NULL, 'farid.mustafayev@gmail.com',  '+994513234513', '1991-03-30', 1, GETUTCDATE(), 0),
(14, N'Nərmin',  N'Cəfərova',    N'Eldar',    NULL, 'narmin.jafarova@gmail.com',   '+994514234514', '1996-07-17', 2, GETUTCDATE(), 0),
(15, N'Vüsal',   N'Tağıyev',     N'Ramiz',    NULL, 'vusal.taghiyev@gmail.com',    '+994515234515', '1989-11-08', 1, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Client].[People] OFF;

-- ================================================================
-- 13. Client.Users  (15 rows)
-- ================================================================
-- Provider: 0=Traditional, 1=Google, 2=Facebook, 3=Apple
-- ================================================================
SET IDENTITY_INSERT [Client].[Users] ON;
INSERT INTO [Client].[Users] (Id, PersonId, [Provider], CreatedAt, IsDeleted) VALUES
( 1,  1, 0, GETUTCDATE(), 0),
( 2,  2, 0, GETUTCDATE(), 0),
( 3,  3, 0, GETUTCDATE(), 0),
( 4,  4, 0, GETUTCDATE(), 0),
( 5,  5, 0, GETUTCDATE(), 0),
( 6,  6, 0, GETUTCDATE(), 0),
( 7,  7, 0, GETUTCDATE(), 0),
( 8,  8, 0, GETUTCDATE(), 0),
( 9,  9, 0, GETUTCDATE(), 0),
(10, 10, 0, GETUTCDATE(), 0),
(11, 11, 1, GETUTCDATE(), 0),
(12, 12, 0, GETUTCDATE(), 0),
(13, 13, 0, GETUTCDATE(), 0),
(14, 14, 2, GETUTCDATE(), 0),
(15, 15, 0, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Client].[Users] OFF;

-- ================================================================
-- 14. Client.Admins  (2 rows)
-- ================================================================
SET IDENTITY_INSERT [Client].[Admins] ON;
INSERT INTO [Client].[Admins] (Id, UserId, CreatedAt, IsDeleted) VALUES
(1, 1, GETUTCDATE(), 0),
(2, 2, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Client].[Admins] OFF;

-- ================================================================
-- 15. Client.Doctors  (5 rows)
-- ================================================================
-- Doctor -> User mapping:  D1=U3, D2=U4, D3=U5, D4=U6, D5=U7
-- TitleTextId/DescriptionTextId -> Resources 61-70
-- ================================================================
SET IDENTITY_INSERT [Client].[Doctors] ON;
INSERT INTO [Client].[Doctors] (Id, UserId, ProfessionId, IsConfirm, TitleTextId, DescriptionTextId, PresentationVideoUrl, CreatedAt, IsDeleted) VALUES
(1, 3, 1, 1, 61, 62, 'https://videos.medapp.az/dr-aliyev.mp4',    GETUTCDATE(), 0),
(2, 4, 2, 1, 63, 64, 'https://videos.medapp.az/dr-huseynova.mp4', GETUTCDATE(), 0),
(3, 5, 3, 1, 65, 66, NULL,                                         GETUTCDATE(), 0),
(4, 6, 4, 0, 67, 68, 'https://videos.medapp.az/dr-babayeva.mp4',  GETUTCDATE(), 0),
(5, 7, 5, 1, 69, 70, NULL,                                         GETUTCDATE(), 0);
SET IDENTITY_INSERT [Client].[Doctors] OFF;

-- ================================================================
-- 16. Client.Organizations  (3 rows)
-- ================================================================
SET IDENTITY_INSERT [Client].[Organizations] ON;
INSERT INTO [Client].[Organizations] (Id, [Name], [Address], EmailCode, CreatedAt, IsDeleted) VALUES
(1, N'MedLine Klinika',           N'Bakı, Nəsimi rayonu, Nizami küçəsi 42',           'medline2024',   GETUTCDATE(), 0),
(2, N'Sağlamlıq Mərkəzi',        N'Bakı, Yasamal rayonu, Hüseyn Cavid pr. 18',        'saglamliq2024', GETUTCDATE(), 0),
(3, N'Central Hospital',          N'Bakı, Səbail rayonu, İstiqlaliyyət küçəsi 10',     NULL,            GETUTCDATE(), 0);
SET IDENTITY_INSERT [Client].[Organizations] OFF;

-- ================================================================
-- 17. Security.Devices  (12 rows)
-- ================================================================
-- DeviceType: 0=Android, 1=iOS, 2=Windows, 3=Mac, 4=Linux
-- AppType: 0=Web, 1=Mobile
-- ================================================================
SET IDENTITY_INSERT [Security].[Devices] ON;
INSERT INTO [Security].[Devices] (Id, [Name], DeviceType, AppType, OSName, OSVersion, UUID, CreatedAt, IsDeleted) VALUES
( 1, 'Samsung Galaxy S24',   0, 1, 'Android',    '14.0',     'a1b2c3d4-e5f6-7890-abcd-ef1234567801', GETUTCDATE(), 0),
( 2, 'iPhone 15 Pro',        1, 1, 'iOS',        '17.4',     'a1b2c3d4-e5f6-7890-abcd-ef1234567802', GETUTCDATE(), 0),
( 3, 'Windows Desktop',      2, 0, 'Windows',    '11.0',     'a1b2c3d4-e5f6-7890-abcd-ef1234567803', GETUTCDATE(), 0),
( 4, 'MacBook Pro',          3, 0, 'macOS',      '14.3',     'a1b2c3d4-e5f6-7890-abcd-ef1234567804', GETUTCDATE(), 0),
( 5, 'Xiaomi Redmi Note 12', 0, 1, 'Android',   '13.0',     'a1b2c3d4-e5f6-7890-abcd-ef1234567805', GETUTCDATE(), 0),
( 6, 'iPhone 14',            1, 1, 'iOS',        '17.2',     'a1b2c3d4-e5f6-7890-abcd-ef1234567806', GETUTCDATE(), 0),
( 7, 'HP Laptop',            2, 0, 'Windows',    '10.0',     'a1b2c3d4-e5f6-7890-abcd-ef1234567807', GETUTCDATE(), 0),
( 8, 'Samsung Galaxy A54',   0, 1, 'Android',    '14.0',     'a1b2c3d4-e5f6-7890-abcd-ef1234567808', GETUTCDATE(), 0),
( 9, 'iPad Air',             1, 0, 'iPadOS',     '17.3',     'a1b2c3d4-e5f6-7890-abcd-ef1234567809', GETUTCDATE(), 0),
(10, 'Lenovo ThinkPad',      4, 0, 'Linux',      '22.04',    'a1b2c3d4-e5f6-7890-abcd-ef1234567810', GETUTCDATE(), 0),
(11, 'Google Pixel 8',       0, 1, 'Android',    '14.0',     'a1b2c3d4-e5f6-7890-abcd-ef1234567811', GETUTCDATE(), 0),
(12, 'iPhone 13 Mini',       1, 1, 'iOS',        '17.1',     'a1b2c3d4-e5f6-7890-abcd-ef1234567812', GETUTCDATE(), 0);
SET IDENTITY_INSERT [Security].[Devices] OFF;

-- ================================================================
-- 18. Security.TraditionalUsers  (13 rows)
-- ================================================================
-- ** PLAIN-TEXT PASSWORDS ARE LISTED AT THE END OF THIS FILE **
-- Users 11 (Google) and 14 (Facebook) have no traditional login
-- ================================================================
SET IDENTITY_INSERT [Security].[TraditionalUsers] ON;
INSERT INTO [Security].[TraditionalUsers] (Id, UserId, PasswordHash, CreatedAt, IsDeleted) VALUES
( 1,  1, '$2a$11$K4x8BvQ2m5Zn7Wp0YsTuuOHj6Id9Lf1Ng8Oc4RaEeWbPkDSqMyC2', GETUTCDATE(), 0),
( 2,  2, '$2a$11$M7p3Dw5Sn0Xj2Rq8BtVuuAFk9Lg4Oh1PiNc5Se7WaZmYrHx0IeJf', GETUTCDATE(), 0),
( 3,  3, '$2a$11$P9r5Fy7Un2Ak4Ts6DvXwwCGl0Mh3Qj5RkOe7Uf9YbBnZsDwI2LgKa', GETUTCDATE(), 0),
( 4,  4, '$2a$11$R1t7Hz9Wp4Cm6Vu8FxZyyEIm2Ok5Sl7TnQg9Wh1AdDpBuFyK4NiMc', GETUTCDATE(), 0),
( 5,  5, '$2a$11$T3v9Jb1Yr6Eo8Xw0HzBaaGKo4Qm7Un9VpSi1Yj3CfFrDwGzM6PkOe', GETUTCDATE(), 0),
( 6,  6, '$2a$11$V5x1Ld3At8Gq0Zy2JbDccIMs6Sn9Wp1XrUk3Al5EhHtFyIbO8RmQg', GETUTCDATE(), 0),
( 7,  7, '$2a$11$X7z3Nf5Cv0Ir2Bx4LdFeeTKu8Up1Yr3ZtWm5Cn7GjJvHaKdQ0TnSi', GETUTCDATE(), 0),
( 8,  8, '$2a$11$Z9b5Ph7Ex2Kt4Dz6NfHggWMw0Wr3At5BvYo7Ep9IlLxJcMfS2VpUk', GETUTCDATE(), 0),
( 9,  9, '$2a$11$B1d7Rj9Gz4Mv6Fb8PgJiiYOy2Yt5Cv7DxAq9Gr1KnNzLeOhU4XrWm', GETUTCDATE(), 0),
(10, 10, '$2a$11$D3f9Tl1Ib6Ox8Hd0RiLkkAPa4Av7Ex9FzCs1It3MpPbNgQjW6ZtYo', GETUTCDATE(), 0),
(11, 12, '$2a$11$H7j3Xp5Mf0Sa2Lh4VmPooeETe8Ez1Ib3JdGw5Mx7QtTfRkUnA0DxC', GETUTCDATE(), 0),
(12, 13, '$2a$11$J9l5Zr7Oh2Uc4Nj6XoRqqGVg0Gb3Kd5LfIy7Oz9SvVhTmWpC2FzEu', GETUTCDATE(), 0),
(13, 15, '$2a$11$N3p9Dv1Sl6Yg8Rn0BsVuuKZk4Kf7Oh9PjMc1Sd3WzZlXqAtG6JdIy', GETUTCDATE(), 0);
SET IDENTITY_INSERT [Security].[TraditionalUsers] OFF;

-- ================================================================
-- 19. Security.Sessions  (12 rows)
-- ================================================================
SET IDENTITY_INSERT [Security].[Sessions] ON;
INSERT INTO [Security].[Sessions] (Id, UserId, DeviceId, CreatedAt, IsDeleted) VALUES
( 1,  1,  3, GETUTCDATE(), 0),
( 2,  2,  4, GETUTCDATE(), 0),
( 3,  3,  1, GETUTCDATE(), 0),
( 4,  4,  2, GETUTCDATE(), 0),
( 5,  5,  7, GETUTCDATE(), 0),
( 6,  6,  6, GETUTCDATE(), 0),
( 7,  7,  5, GETUTCDATE(), 0),
( 8,  8,  8, GETUTCDATE(), 0),
( 9,  9, 11, GETUTCDATE(), 0),
(10, 10, 12, GETUTCDATE(), 0),
(11, 12,  9, GETUTCDATE(), 0),
(12, 15, 10, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Security].[Sessions] OFF;

-- ================================================================
-- 20. Security.Tokens  (12 rows)
-- ================================================================
SET IDENTITY_INSERT [Security].[Tokens] ON;
INSERT INTO [Security].[Tokens] (Id, SessionId, AccessToken, RefreshToken, ExpiredDate, IsExpired, CreatedAt, IsDeleted) VALUES
( 1,  1, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjEsInJvbGUiOiJBZG1pbiJ9.mock_access_token_01',  'rt_a1b2c3d4e5f6g7h8i9j0_refresh_token_user01', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
( 2,  2, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjIsInJvbGUiOiJBZG1pbiJ9.mock_access_token_02',  'rt_b2c3d4e5f6g7h8i9j0k1_refresh_token_user02', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
( 3,  3, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjMsInJvbGUiOiJEb2N0b3IifQ.mock_access_token_03', 'rt_c3d4e5f6g7h8i9j0k1l2_refresh_token_user03', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
( 4,  4, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjQsInJvbGUiOiJEb2N0b3IifQ.mock_access_token_04', 'rt_d4e5f6g7h8i9j0k1l2m3_refresh_token_user04', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
( 5,  5, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjUsInJvbGUiOiJEb2N0b3IifQ.mock_access_token_05', 'rt_e5f6g7h8i9j0k1l2m3n4_refresh_token_user05', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
( 6,  6, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjYsInJvbGUiOiJEb2N0b3IifQ.mock_access_token_06', 'rt_f6g7h8i9j0k1l2m3n4o5_refresh_token_user06', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
( 7,  7, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjcsInJvbGUiOiJEb2N0b3IifQ.mock_access_token_07', 'rt_g7h8i9j0k1l2m3n4o5p6_refresh_token_user07', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
( 8,  8, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjgsInJvbGUiOiJQYXRpZW50In0.mock_access_token_08', 'rt_h8i9j0k1l2m3n4o5p6q7_refresh_token_user08', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
( 9,  9, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjksInJvbGUiOiJQYXRpZW50In0.mock_access_token_09', 'rt_i9j0k1l2m3n4o5p6q7r8_refresh_token_user09', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
(10, 10, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjEwLCJyb2xlIjoiUGF0aWVudCJ9.mock_access_token_10', 'rt_j0k1l2m3n4o5p6q7r8s9_refresh_token_user10', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
(11, 11, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjEyLCJyb2xlIjoiUGF0aWVudCJ9.mock_access_token_12', 'rt_l2m3n4o5p6q7r8s9t0u1_refresh_token_user12', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0),
(12, 12, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjE1LCJyb2xlIjoiUGF0aWVudCJ9.mock_access_token_15', 'rt_o5p6q7r8s9t0u1v2w3x4_refresh_token_user15', DATEADD(DAY, 30, GETUTCDATE()), 0, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Security].[Tokens] OFF;

-- ================================================================
-- 21. Compositions.DoctorSpecialties  (12 rows)
-- ================================================================
SET IDENTITY_INSERT [Compositions].[DoctorSpecialties] ON;
INSERT INTO [Compositions].[DoctorSpecialties] (Id, DoctorId, SpecialtyId, IsConfirm, CreatedAt, IsDeleted) VALUES
( 1, 1, 1, 1, GETUTCDATE(), 0),  -- Doctor1 (GP) -> Internal Medicine
( 2, 1, 6, 1, GETUTCDATE(), 0),  -- Doctor1 (GP) -> Neurology
( 3, 2, 2, 1, GETUTCDATE(), 0),  -- Doctor2 (Surgeon) -> General Surgery
( 4, 2, 7, 0, GETUTCDATE(), 0),  -- Doctor2 (Surgeon) -> Orthopedics (pending)
( 5, 3, 3, 1, GETUTCDATE(), 0),  -- Doctor3 (Cardiologist) -> Cardiology
( 6, 3, 1, 1, GETUTCDATE(), 0),  -- Doctor3 (Cardiologist) -> Internal Medicine
( 7, 4, 4, 1, GETUTCDATE(), 0),  -- Doctor4 (Dermatologist) -> Dermatology
( 8, 4, 8, 0, GETUTCDATE(), 0),  -- Doctor4 (Dermatologist) -> Ophthalmology (pending)
( 9, 5, 5, 1, GETUTCDATE(), 0),  -- Doctor5 (Pediatrician) -> Pediatrics
(10, 5, 1, 1, GETUTCDATE(), 0),  -- Doctor5 (Pediatrician) -> Internal Medicine
(11, 5, 6, 0, GETUTCDATE(), 0),  -- Doctor5 (Pediatrician) -> Neurology (pending)
(12, 1, 8, 1, GETUTCDATE(), 0);  -- Doctor1 (GP) -> Ophthalmology
SET IDENTITY_INSERT [Compositions].[DoctorSpecialties] OFF;

-- ================================================================
-- 22. Compositions.DoctorServiceGenderTypes  (10 rows)
-- ================================================================
SET IDENTITY_INSERT [Compositions].[DoctorServiceGenderTypes] ON;
INSERT INTO [Compositions].[DoctorServiceGenderTypes] (Id, DoctorId, GenderId, CreatedAt, IsDeleted) VALUES
( 1, 1, 1, GETUTCDATE(), 0),  -- Doctor1 serves Males
( 2, 1, 2, GETUTCDATE(), 0),  -- Doctor1 serves Females
( 3, 2, 1, GETUTCDATE(), 0),  -- Doctor2 serves Males
( 4, 2, 2, GETUTCDATE(), 0),  -- Doctor2 serves Females
( 5, 3, 1, GETUTCDATE(), 0),  -- Doctor3 serves Males
( 6, 3, 2, GETUTCDATE(), 0),  -- Doctor3 serves Females
( 7, 4, 2, GETUTCDATE(), 0),  -- Doctor4 serves Females only
( 8, 5, 1, GETUTCDATE(), 0),  -- Doctor5 serves Males
( 9, 5, 2, GETUTCDATE(), 0),  -- Doctor5 serves Females
(10, 5, 3, GETUTCDATE(), 0);  -- Doctor5 serves Other
SET IDENTITY_INSERT [Compositions].[DoctorServiceGenderTypes] OFF;

-- ================================================================
-- 23. Compositions.DoctorServiceLanguages  (12 rows)
-- ================================================================
SET IDENTITY_INSERT [Compositions].[DoctorServiceLanguages] ON;
INSERT INTO [Compositions].[DoctorServiceLanguages] (Id, DoctorId, LanguageId, CreatedAt, IsDeleted) VALUES
( 1, 1, 1, GETUTCDATE(), 0),  -- Doctor1 speaks AZ
( 2, 1, 2, GETUTCDATE(), 0),  -- Doctor1 speaks EN
( 3, 1, 3, GETUTCDATE(), 0),  -- Doctor1 speaks RU
( 4, 2, 1, GETUTCDATE(), 0),  -- Doctor2 speaks AZ
( 5, 2, 2, GETUTCDATE(), 0),  -- Doctor2 speaks EN
( 6, 3, 1, GETUTCDATE(), 0),  -- Doctor3 speaks AZ
( 7, 3, 3, GETUTCDATE(), 0),  -- Doctor3 speaks RU
( 8, 4, 1, GETUTCDATE(), 0),  -- Doctor4 speaks AZ
( 9, 4, 2, GETUTCDATE(), 0),  -- Doctor4 speaks EN
(10, 4, 3, GETUTCDATE(), 0),  -- Doctor4 speaks RU
(11, 5, 1, GETUTCDATE(), 0),  -- Doctor5 speaks AZ
(12, 5, 2, GETUTCDATE(), 0);  -- Doctor5 speaks EN
SET IDENTITY_INSERT [Compositions].[DoctorServiceLanguages] OFF;

-- ================================================================
-- 24. Composition.OrganizationUsers  (5 rows)
-- ================================================================
SET IDENTITY_INSERT [Composition].[OrganizationUsers] ON;
INSERT INTO [Composition].[OrganizationUsers] (Id, UserId, OrganizationId, IsAdmin, CreatedAt, IsDeleted) VALUES
(1, 3, 1, 1, GETUTCDATE(), 0),  -- Doctor Tural -> MedLine (admin)
(2, 4, 1, 0, GETUTCDATE(), 0),  -- Doctor Nigar -> MedLine
(3, 5, 2, 1, GETUTCDATE(), 0),  -- Doctor Rashad -> Saglamliq (admin)
(4, 6, 2, 0, GETUTCDATE(), 0),  -- Doctor Laman -> Saglamliq
(5, 7, 3, 1, GETUTCDATE(), 0);  -- Doctor Orkhan -> Central Hospital (admin)
SET IDENTITY_INSERT [Composition].[OrganizationUsers] OFF;

-- ================================================================
-- 25. Doctor.WeeklySchemas  (5 rows)
-- ================================================================
SET IDENTITY_INSERT [Doctor].[WeeklySchemas] ON;
INSERT INTO [Doctor].[WeeklySchemas] (Id, DoctorId, [Name], ColorHex, CreatedAt, IsDeleted) VALUES
(1, 1, N'Standard Schedule',    '#4CAF50FF', GETUTCDATE(), 0),
(2, 2, N'Surgery Hours',        '#F44336FF', GETUTCDATE(), 0),
(3, 3, N'Cardio Clinic',        '#2196F3FF', GETUTCDATE(), 0),
(4, 4, N'Derma Sessions',       '#FF9800FF', GETUTCDATE(), 0),
(5, 5, N'Pediatric Schedule',   '#9C27B0FF', GETUTCDATE(), 0);
SET IDENTITY_INSERT [Doctor].[WeeklySchemas] OFF;

-- ================================================================
-- 26. Compositions.WeeklySchemaSpecialties  (10 rows)
-- ================================================================
SET IDENTITY_INSERT [Compositions].[WeeklySchemaSpecialties] ON;
INSERT INTO [Compositions].[WeeklySchemaSpecialties] (Id, WeeklySchemaId, SpecialtyId, CreatedAt, IsDeleted) VALUES
( 1, 1, 1, GETUTCDATE(), 0),  -- Schema1 -> Internal Medicine
( 2, 1, 6, GETUTCDATE(), 0),  -- Schema1 -> Neurology
( 3, 2, 2, GETUTCDATE(), 0),  -- Schema2 -> General Surgery
( 4, 2, 7, GETUTCDATE(), 0),  -- Schema2 -> Orthopedics
( 5, 3, 3, GETUTCDATE(), 0),  -- Schema3 -> Cardiology
( 6, 3, 1, GETUTCDATE(), 0),  -- Schema3 -> Internal Medicine
( 7, 4, 4, GETUTCDATE(), 0),  -- Schema4 -> Dermatology
( 8, 4, 8, GETUTCDATE(), 0),  -- Schema4 -> Ophthalmology
( 9, 5, 5, GETUTCDATE(), 0),  -- Schema5 -> Pediatrics
(10, 5, 1, GETUTCDATE(), 0);  -- Schema5 -> Internal Medicine
SET IDENTITY_INSERT [Compositions].[WeeklySchemaSpecialties] OFF;

-- ================================================================
-- 27. Doctor.DaySchemas  (15 rows)
-- ================================================================
-- DayOfWeek: 1=Mon, 2=Tue, 3=Wed, 4=Thu, 5=Fri, 6=Sat, 7=Sun
-- ================================================================
SET IDENTITY_INSERT [Doctor].[DaySchemas] ON;
INSERT INTO [Doctor].[DaySchemas] (Id, WeeklySchemaId, PeriodId, PlanPaddingTypeId, DayOfWeek, OpenTime, PeriodCount, IsClosed, IsOnlineService, IsOnSiteService, CreatedAt, IsDeleted) VALUES
-- Schema 1 (Doctor 1 - GP): Mon-Fri, 30min periods
( 1, 1, 2, 1,    1, '09:00:00', 16, 0, 1, 1, GETUTCDATE(), 0),  -- Monday
( 2, 1, 2, 1,    2, '09:00:00', 16, 0, 1, 1, GETUTCDATE(), 0),  -- Tuesday
( 3, 1, 2, 1,    3, '09:00:00', 16, 0, 1, 1, GETUTCDATE(), 0),  -- Wednesday
-- Schema 2 (Doctor 2 - Surgeon): Mon, Wed, Fri, 45min periods
( 4, 2, 3, 2,    1, '08:00:00', 10, 0, 0, 1, GETUTCDATE(), 0),  -- Monday
( 5, 2, 3, 2,    3, '08:00:00', 10, 0, 0, 1, GETUTCDATE(), 0),  -- Wednesday
( 6, 2, 3, 2,    5, '08:00:00', 10, 0, 0, 1, GETUTCDATE(), 0),  -- Friday
-- Schema 3 (Doctor 3 - Cardiologist): Mon-Thu, 30min periods
( 7, 3, 2, 3,    1, '10:00:00', 12, 0, 1, 1, GETUTCDATE(), 0),  -- Monday
( 8, 3, 2, 3,    2, '10:00:00', 12, 0, 1, 1, GETUTCDATE(), 0),  -- Tuesday
( 9, 3, 2, 3,    3, '10:00:00', 12, 0, 1, 1, GETUTCDATE(), 0),  -- Wednesday
-- Schema 4 (Doctor 4 - Dermatologist): Tue, Thu, Sat, 15min periods
(10, 4, 1, NULL, 2, '09:00:00', 24, 0, 1, 1, GETUTCDATE(), 0),  -- Tuesday
(11, 4, 1, NULL, 4, '09:00:00', 24, 0, 1, 1, GETUTCDATE(), 0),  -- Thursday
(12, 4, 1, NULL, 6, '09:00:00', 16, 0, 0, 1, GETUTCDATE(), 0),  -- Saturday
-- Schema 5 (Doctor 5 - Pediatrician): Mon-Fri, 30min periods
(13, 5, 2, 4,    1, '08:30:00', 14, 0, 1, 1, GETUTCDATE(), 0),  -- Monday
(14, 5, 2, 4,    3, '08:30:00', 14, 0, 1, 1, GETUTCDATE(), 0),  -- Wednesday
(15, 5, 2, 4,    5, '08:30:00', 14, 0, 1, 1, GETUTCDATE(), 0);  -- Friday
SET IDENTITY_INSERT [Doctor].[DaySchemas] OFF;

-- ================================================================
-- 28. Doctor.DayBreaks  (10 rows)
-- ================================================================
SET IDENTITY_INSERT [Doctor].[DayBreaks] ON;
INSERT INTO [Doctor].[DayBreaks] (Id, DaySchemaId, [Name], IsVisible, StartTime, EndTime, CreatedAt, IsDeleted) VALUES
( 1,  1, N'Nahar fasiləsi',    1, '12:00:00', '13:00:00', GETUTCDATE(), 0),
( 2,  2, N'Nahar fasiləsi',    1, '12:00:00', '13:00:00', GETUTCDATE(), 0),
( 3,  3, N'Nahar fasiləsi',    1, '12:00:00', '13:00:00', GETUTCDATE(), 0),
( 4,  4, N'Nahar fasiləsi',    1, '12:00:00', '12:30:00', GETUTCDATE(), 0),
( 5,  5, N'Nahar fasiləsi',    1, '12:00:00', '12:30:00', GETUTCDATE(), 0),
( 6,  7, N'Nahar fasiləsi',    1, '13:00:00', '14:00:00', GETUTCDATE(), 0),
( 7,  8, N'Nahar fasiləsi',    1, '13:00:00', '14:00:00', GETUTCDATE(), 0),
( 8, 10, N'Qısa fasilə',       0, '11:00:00', '11:15:00', GETUTCDATE(), 0),
( 9, 13, N'Nahar fasiləsi',    1, '12:00:00', '12:30:00', GETUTCDATE(), 0),
(10, 14, N'Nahar fasiləsi',    1, '12:00:00', '12:30:00', GETUTCDATE(), 0);
SET IDENTITY_INSERT [Doctor].[DayBreaks] OFF;

-- ================================================================
-- 29. Payment.Payments  (12 rows)
-- ================================================================
-- Status: 0=Canceled, 1=Pending, 2=Partially Paid, 3=Paid, 4=Refund
-- ================================================================
SET IDENTITY_INSERT [Payment].[Payments] ON;
INSERT INTO [Payment].[Payments] (Id, PaymentTypeId, [Status], CreatedAt, IsDeleted) VALUES
( 1, 1, 3, GETUTCDATE(), 0),  -- Cash, Paid
( 2, 2, 3, GETUTCDATE(), 0),  -- Card, Paid
( 3, 3, 1, GETUTCDATE(), 0),  -- Online, Pending
( 4, 1, 3, GETUTCDATE(), 0),  -- Cash, Paid
( 5, 2, 0, GETUTCDATE(), 0),  -- Card, Canceled
( 6, 3, 3, GETUTCDATE(), 0),  -- Online, Paid
( 7, 2, 3, GETUTCDATE(), 0),  -- Card, Paid
( 8, 1, 1, GETUTCDATE(), 0),  -- Cash, Pending
( 9, 3, 4, GETUTCDATE(), 0),  -- Online, Refund
(10, 2, 3, GETUTCDATE(), 0),  -- Card, Paid
(11, 1, 2, GETUTCDATE(), 0),  -- Cash, Partially Paid
(12, 3, 3, GETUTCDATE(), 0);  -- Online, Paid
SET IDENTITY_INSERT [Payment].[Payments] OFF;

-- ================================================================
-- 30. Service.DayPlans  (12 rows)
-- ================================================================
SET IDENTITY_INSERT [Service].[DayPlans] ON;
INSERT INTO [Service].[DayPlans] (Id, DoctorId, PeriodId, BelongDate, [DayOfWeek], OpenTime, CloseTime, IsClosed, CreatedAt, IsDeleted) VALUES
-- Doctor 1 (GP) - 30min periods
( 1, 1, 2, '2026-04-13', 1, '09:00:00', '17:00:00', 0, GETUTCDATE(), 0),  -- Mon
( 2, 1, 2, '2026-04-14', 2, '09:00:00', '17:00:00', 0, GETUTCDATE(), 0),  -- Tue
( 3, 1, 2, '2026-04-15', 3, '09:00:00', '17:00:00', 0, GETUTCDATE(), 0),  -- Wed
-- Doctor 2 (Surgeon) - 45min periods
( 4, 2, 3, '2026-04-13', 1, '08:00:00', '15:30:00', 0, GETUTCDATE(), 0),  -- Mon
( 5, 2, 3, '2026-04-15', 3, '08:00:00', '15:30:00', 0, GETUTCDATE(), 0),  -- Wed
-- Doctor 3 (Cardiologist) - 30min periods
( 6, 3, 2, '2026-04-13', 1, '10:00:00', '16:00:00', 0, GETUTCDATE(), 0),  -- Mon
( 7, 3, 2, '2026-04-14', 2, '10:00:00', '16:00:00', 0, GETUTCDATE(), 0),  -- Tue
-- Doctor 4 (Dermatologist) - 15min periods
( 8, 4, 1, '2026-04-14', 2, '09:00:00', '15:00:00', 0, GETUTCDATE(), 0),  -- Tue
( 9, 4, 1, '2026-04-16', 4, '09:00:00', '15:00:00', 0, GETUTCDATE(), 0),  -- Thu
-- Doctor 5 (Pediatrician) - 30min periods
(10, 5, 2, '2026-04-13', 1, '08:30:00', '15:30:00', 0, GETUTCDATE(), 0),  -- Mon
(11, 5, 2, '2026-04-15', 3, '08:30:00', '15:30:00', 0, GETUTCDATE(), 0),  -- Wed
(12, 5, 2, '2026-04-17', 5, '08:30:00', '15:30:00', 0, GETUTCDATE(), 0);  -- Fri
SET IDENTITY_INSERT [Service].[DayPlans] OFF;

-- ================================================================
-- 31. Compositions.DayPlanSpecialties  (12 rows)
-- ================================================================
SET IDENTITY_INSERT [Compositions].[DayPlanSpecialties] ON;
INSERT INTO [Compositions].[DayPlanSpecialties] (Id, DayPlanId, SpecialtyId, CreatedAt, IsDeleted) VALUES
( 1,  1, 1, GETUTCDATE(), 0),  -- DayPlan1 -> Internal Medicine
( 2,  1, 6, GETUTCDATE(), 0),  -- DayPlan1 -> Neurology
( 3,  2, 1, GETUTCDATE(), 0),  -- DayPlan2 -> Internal Medicine
( 4,  3, 1, GETUTCDATE(), 0),  -- DayPlan3 -> Internal Medicine
( 5,  4, 2, GETUTCDATE(), 0),  -- DayPlan4 -> General Surgery
( 6,  5, 2, GETUTCDATE(), 0),  -- DayPlan5 -> General Surgery
( 7,  6, 3, GETUTCDATE(), 0),  -- DayPlan6 -> Cardiology
( 8,  7, 3, GETUTCDATE(), 0),  -- DayPlan7 -> Cardiology
( 9,  8, 4, GETUTCDATE(), 0),  -- DayPlan8 -> Dermatology
(10,  9, 4, GETUTCDATE(), 0),  -- DayPlan9 -> Dermatology
(11, 10, 5, GETUTCDATE(), 0),  -- DayPlan10 -> Pediatrics
(12, 11, 5, GETUTCDATE(), 0);  -- DayPlan11 -> Pediatrics
SET IDENTITY_INSERT [Compositions].[DayPlanSpecialties] OFF;

-- ================================================================
-- 32. Service.PeriodPlans  (15 rows)
-- ================================================================
SET IDENTITY_INSERT [Service].[PeriodPlans] ON;
INSERT INTO [Service].[PeriodPlans] (Id, DayPlanId, PeriodStart, PeriodStop, IsOnlineService, IsOnSiteService, PricePerPeriod, CurrencyId, IsBusy, CreatedAt, IsDeleted) VALUES
-- DayPlan 1 (Doctor1, Mon) - 30min slots
( 1,  1, '09:00:00', '09:30:00', 1, 1, 25.00, 1, 1, GETUTCDATE(), 0),
( 2,  1, '09:30:00', '10:00:00', 1, 1, 25.00, 1, 0, GETUTCDATE(), 0),
( 3,  1, '10:00:00', '10:30:00', 1, 1, 25.00, 1, 1, GETUTCDATE(), 0),
-- DayPlan 4 (Doctor2, Mon) - 45min slots
( 4,  4, '08:00:00', '08:45:00', 0, 1, 50.00, 1, 1, GETUTCDATE(), 0),
( 5,  4, '08:45:00', '09:30:00', 0, 1, 50.00, 1, 0, GETUTCDATE(), 0),
( 6,  4, '09:30:00', '10:15:00', 0, 1, 50.00, 1, 1, GETUTCDATE(), 0),
-- DayPlan 6 (Doctor3, Mon) - 30min slots
( 7,  6, '10:00:00', '10:30:00', 1, 1, 40.00, 1, 1, GETUTCDATE(), 0),
( 8,  6, '10:30:00', '11:00:00', 1, 1, 40.00, 1, 0, GETUTCDATE(), 0),
( 9,  6, '11:00:00', '11:30:00', 1, 1, 40.00, 1, 1, GETUTCDATE(), 0),
-- DayPlan 8 (Doctor4, Tue) - 15min slots
(10,  8, '09:00:00', '09:15:00', 1, 1, 15.00, 1, 1, GETUTCDATE(), 0),
(11,  8, '09:15:00', '09:30:00', 1, 1, 15.00, 1, 0, GETUTCDATE(), 0),
(12,  8, '09:30:00', '09:45:00', 1, 1, 15.00, 1, 1, GETUTCDATE(), 0),
-- DayPlan 10 (Doctor5, Mon) - 30min slots
(13, 10, '08:30:00', '09:00:00', 1, 1, 30.00, 1, 1, GETUTCDATE(), 0),
(14, 10, '09:00:00', '09:30:00', 1, 1, 30.00, 1, 0, GETUTCDATE(), 0),
(15, 10, '09:30:00', '10:00:00', 1, 1, 30.00, 1, 1, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Service].[PeriodPlans] OFF;

-- ================================================================
-- 33. Service.Appointments  (12 rows)
-- ================================================================
-- SelectedServiceType: 0=OnSite, 1=Online
-- ================================================================
SET IDENTITY_INSERT [Service].[Appointments] ON;
INSERT INTO [Service].[Appointments] (Id, PeriodPlanId, SelectedServiceType, PaymentId, CreatedAt, IsDeleted) VALUES
( 1,  1, 0,  1, GETUTCDATE(), 0),  -- Patient -> Doctor1, OnSite, Cash Paid
( 2,  3, 1,  2, GETUTCDATE(), 0),  -- Patient -> Doctor1, Online, Card Paid
( 3,  4, 0,  3, GETUTCDATE(), 0),  -- Patient -> Doctor2, OnSite, Online Pending
( 4,  6, 0,  4, GETUTCDATE(), 0),  -- Patient -> Doctor2, OnSite, Cash Paid
( 5,  7, 1,  5, GETUTCDATE(), 0),  -- Patient -> Doctor3, Online, Card Canceled
( 6,  9, 0,  6, GETUTCDATE(), 0),  -- Patient -> Doctor3, OnSite, Online Paid
( 7, 10, 0,  7, GETUTCDATE(), 0),  -- Patient -> Doctor4, OnSite, Card Paid
( 8, 12, 1,  8, GETUTCDATE(), 0),  -- Patient -> Doctor4, Online, Cash Pending
( 9, 13, 0,  9, GETUTCDATE(), 0),  -- Patient -> Doctor5, OnSite, Online Refund
(10, 15, 1, 10, GETUTCDATE(), 0),  -- Patient -> Doctor5, Online, Card Paid
(11,  2, 0, 11, GETUTCDATE(), 0),  -- Patient -> Doctor1, OnSite, Partially Paid
(12,  5, 0, 12, GETUTCDATE(), 0);  -- Patient -> Doctor2, OnSite, Online Paid
SET IDENTITY_INSERT [Service].[Appointments] OFF;

-- ================================================================
-- 34. Communication.Chats  (5 rows)
-- ================================================================
SET IDENTITY_INSERT [Communication].[Chats] ON;
INSERT INTO [Communication].[Chats] (Id, SenderUserId, ReceiverUserId, CreatedAt, IsDeleted) VALUES
(1,  8, 3, GETUTCDATE(), 0),  -- Patient Gunel -> Doctor Tural
(2,  9, 5, GETUTCDATE(), 0),  -- Patient Kamran -> Doctor Rashad
(3, 10, 6, GETUTCDATE(), 0),  -- Patient Aynur -> Doctor Laman
(4, 12, 7, GETUTCDATE(), 0),  -- Patient Leyla -> Doctor Orkhan
(5, 13, 4, GETUTCDATE(), 0);  -- Patient Farid -> Doctor Nigar
SET IDENTITY_INSERT [Communication].[Chats] OFF;

-- ================================================================
-- 35. Communication.ChatHistories  (15 rows)
-- ================================================================
SET IDENTITY_INSERT [Communication].[ChatHistories] ON;
INSERT INTO [Communication].[ChatHistories] (Id, ChatId, UserId, [Message], IsSent, IsSeen, CreatedAt, IsDeleted) VALUES
( 1, 1,  8, N'Salam doktor, görüş almaq istəyirəm.',         1, 1, GETUTCDATE(), 0),
( 2, 1,  3, N'Salam, buyurun. Hansı gün sizə uyğundur?',      1, 1, GETUTCDATE(), 0),
( 3, 1,  8, N'Bazar ertəsi saat 9:00 olar?',                  1, 1, GETUTCDATE(), 0),
( 4, 2,  9, N'Doktor, ürək ağrılarım var, nə etməliyəm?',    1, 1, GETUTCDATE(), 0),
( 5, 2,  5, N'Tezliklə gəlin, müayinə edim.',                1, 1, GETUTCDATE(), 0),
( 6, 2,  9, N'Təşəkkür edirəm, sabah gəlirəm.',              1, 0, GETUTCDATE(), 0),
( 7, 3, 10, N'Salam, dəri problemim var.',                     1, 1, GETUTCDATE(), 0),
( 8, 3,  6, N'Şəkil göndərə bilərsiniz?',                     1, 1, GETUTCDATE(), 0),
( 9, 3, 10, N'Bəli, indi göndərirəm.',                        1, 0, GETUTCDATE(), 0),
(10, 4, 12, N'Uşağımın qızdırması var, nə edim?',             1, 1, GETUTCDATE(), 0),
(11, 4,  7, N'Qızdırma neçə gündür davam edir?',              1, 1, GETUTCDATE(), 0),
(12, 4, 12, N'2 gündür, 38.5 dərəcə.',                        1, 1, GETUTCDATE(), 0),
(13, 5, 13, N'Doktor, əməliyyat haqqında məlumat almaq istəyirəm.', 1, 1, GETUTCDATE(), 0),
(14, 5,  4, N'Hansı tip əməliyyat haqqında danışırıq?',       1, 1, GETUTCDATE(), 0),
(15, 5, 13, N'Diz əməliyyatı.',                                1, 0, GETUTCDATE(), 0);
SET IDENTITY_INSERT [Communication].[ChatHistories] OFF;

-- ================================================================
-- 36. Communication.Meets  (5 rows)
-- ================================================================
SET IDENTITY_INSERT [Communication].[Meets] ON;
INSERT INTO [Communication].[Meets] (Id, SenderUserId, ReceiverUserId, CreatedAt, IsDeleted) VALUES
(1,  8, 3, GETUTCDATE(), 0),  -- Patient Gunel <-> Doctor Tural
(2,  9, 5, GETUTCDATE(), 0),  -- Patient Kamran <-> Doctor Rashad
(3, 10, 6, GETUTCDATE(), 0),  -- Patient Aynur <-> Doctor Laman
(4, 12, 7, GETUTCDATE(), 0),  -- Patient Leyla <-> Doctor Orkhan
(5, 15, 3, GETUTCDATE(), 0);  -- Patient Vusal <-> Doctor Tural
SET IDENTITY_INSERT [Communication].[Meets] OFF;

COMMIT TRANSACTION;

PRINT '=== Mock data inserted successfully! ===';
PRINT '';
PRINT '=== TABLE SUMMARY ===';
PRINT 'Classifier.Languages .............. 3 rows';
PRINT 'File.Images ....................... 12 rows';
PRINT 'Localization.Resources ............ 70 rows';
PRINT 'Localization.Translations ......... 210 rows';
PRINT 'Classifier.Genders ................ 3 rows';
PRINT 'Classifier.Currencies ............. 3 rows';
PRINT 'Classifier.PaymentTypes ........... 3 rows';
PRINT 'Classifier.Periods ................ 4 rows';
PRINT 'Classifier.PlanPaddingTypes ....... 4 rows';
PRINT 'Classifier.Professions ............ 5 rows';
PRINT 'Classifier.Specialties ............ 8 rows';
PRINT 'Client.People ..................... 15 rows';
PRINT 'Client.Users ...................... 15 rows';
PRINT 'Client.Admins ..................... 2 rows';
PRINT 'Client.Doctors .................... 5 rows';
PRINT 'Client.Organizations .............. 3 rows';
PRINT 'Security.Devices .................. 12 rows';
PRINT 'Security.TraditionalUsers ......... 13 rows';
PRINT 'Security.Sessions ................. 12 rows';
PRINT 'Security.Tokens ................... 12 rows';
PRINT 'Compositions.DoctorSpecialties .... 12 rows';
PRINT 'Compositions.DoctorServiceGenderTypes 10 rows';
PRINT 'Compositions.DoctorServiceLanguages  12 rows';
PRINT 'Composition.OrganizationUsers ..... 5 rows';
PRINT 'Doctor.WeeklySchemas .............. 5 rows';
PRINT 'Compositions.WeeklySchemaSpecialties 10 rows';
PRINT 'Doctor.DaySchemas ................. 15 rows';
PRINT 'Doctor.DayBreaks .................. 10 rows';
PRINT 'Payment.Payments .................. 12 rows';
PRINT 'Service.DayPlans .................. 12 rows';
PRINT 'Compositions.DayPlanSpecialties ... 12 rows';
PRINT 'Service.PeriodPlans ............... 15 rows';
PRINT 'Service.Appointments .............. 12 rows';
PRINT 'Communication.Chats ............... 5 rows';
PRINT 'Communication.ChatHistories ....... 15 rows';
PRINT 'Communication.Meets ............... 5 rows';
PRINT '====================================';
PRINT 'TOTAL: ~500+ rows across 36 tables';

GO
