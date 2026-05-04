namespace MedAppointment.DataTransferObjects.FileDtos
{
    public class DocumentUploadMetaDto
    {
        public long DoctorId { get; set; }
        public long? SpecialtyId { get; set; }
        public bool IsProfessionBackground { get; set; }
        public bool IsExperience { get; set; }
        public string? Title { get; set; }
        public string? Issuer { get; set; }
        public string? PeriodOfYear { get; set; }
        public string? Description { get; set; }
    }

    public class DocumentUploadResultDto
    {
        public Guid DocumentId { get; set; }
    }

    public class DocumentInfoDto
    {
        public Guid DocumentId { get; set; }
        public long DoctorId { get; set; }
        public long? SpecialtyId { get; set; }
        public bool IsProfessionBackground { get; set; }
        public bool IsExperience { get; set; }
        public string Filename { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string? Title { get; set; }
        public string? Issuer { get; set; }
        public string? PeriodOfYear { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DocumentDownloadDto
    {
        public string FilePath { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string Filename { get; set; } = null!;
    }
}
