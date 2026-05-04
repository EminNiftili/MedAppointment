namespace MedAppointment.Logic.Tests.Magic;

public static class MagicDocument
{
    public static readonly Guid DocumentGuid = new("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
    public static readonly Guid DocumentGuidNonExistent = new("ffffffff-ffff-ffff-ffff-ffffffffffff");

    public static DocumentUploadMetaDto ValidMetaWithSpecialty => new()
    {
        DoctorId = MagicIds.DoctorIdOne,
        SpecialtyId = MagicIds.SpecialtyIdOne,
        IsProfessionBackground = false,
        IsExperience = false,
        Title = "Cardiology Certificate",
        Issuer = "Ministry of Health",
        PeriodOfYear = "2023",
        Description = "Official cardiology specialization certificate.",
    };

    public static DocumentUploadMetaDto ValidMetaWithProfessionBackground => new()
    {
        DoctorId = MagicIds.DoctorIdOne,
        SpecialtyId = null,
        IsProfessionBackground = true,
        IsExperience = false,
        Title = "Medical Diploma",
        Issuer = "Baku Medical University",
        PeriodOfYear = "2015",
        Description = "Bachelor of Medicine diploma.",
    };

    public static DocumentUploadMetaDto ValidMetaWithExperience => new()
    {
        DoctorId = MagicIds.DoctorIdOne,
        SpecialtyId = null,
        IsProfessionBackground = false,
        IsExperience = true,
        Title = "Work Experience",
        Issuer = "City Hospital",
        PeriodOfYear = "2018-2023",
        Description = "5 years of clinical experience.",
    };

    public static DocumentUploadMetaDto InvalidMetaNoTypeSet => new()
    {
        DoctorId = MagicIds.DoctorIdOne,
        SpecialtyId = null,
        IsProfessionBackground = false,
        IsExperience = false,
    };

    public static DocumentUploadMetaDto InvalidMetaMultipleTypesSet => new()
    {
        DoctorId = MagicIds.DoctorIdOne,
        SpecialtyId = MagicIds.SpecialtyIdOne,
        IsProfessionBackground = true,
        IsExperience = false,
    };

    public static DocumentUploadMetaDto InvalidMetaZeroDoctorId => new()
    {
        DoctorId = 0,
        SpecialtyId = MagicIds.SpecialtyIdOne,
        IsProfessionBackground = false,
        IsExperience = false,
    };

    public static DocumentEntity EntityWithSpecialty => new()
    {
        Id = MagicIds.DocumentIdOne,
        DocumentId = DocumentGuid,
        DoctorId = MagicIds.DoctorIdOne,
        SpecialtyId = MagicIds.SpecialtyIdOne,
        IsProfessionBackground = false,
        IsExperience = false,
        Filename = "certificate.pdf",
        MimeType = "application/pdf",
        FilePath = @"C:\MedAppointmentApp\certificate.pdf",
        Title = "Cardiology Certificate",
        Issuer = "Ministry of Health",
        PeriodOfYear = "2023",
        Description = "Official certificate.",
        CreatedAt = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc),
        IsDeleted = false,
    };

    public static DocumentEntity EntityWithProfessionBackground => new()
    {
        Id = MagicIds.DocumentIdTwo,
        DocumentId = new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
        DoctorId = MagicIds.DoctorIdOne,
        SpecialtyId = null,
        IsProfessionBackground = true,
        IsExperience = false,
        Filename = "diploma.jpg",
        MimeType = "image/jpeg",
        FilePath = @"C:\MedAppointmentApp\diploma.jpg",
        Title = "Medical Diploma",
        Issuer = "Baku Medical University",
        PeriodOfYear = "2015",
        Description = "Bachelor of Medicine.",
        CreatedAt = new DateTime(2024, 2, 20, 9, 0, 0, DateTimeKind.Utc),
        IsDeleted = false,
    };
}
