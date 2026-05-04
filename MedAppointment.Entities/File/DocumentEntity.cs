namespace MedAppointment.Entities.File
{
    public class DocumentEntity : BaseEntity
    {
        public Guid DocumentId { get; set; }
        public long DoctorId { get; set; }
        public long? SpecialtyId { get; set; }
        public bool IsProfessionBackground { get; set; }
        public bool IsExperience { get; set; }
        public string Filename { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        /// <summary>
        /// Physical file path on the server.
        /// </summary>
        public string FilePath { get; set; } = null!;
        public string? Title { get; set; }
        public string? Issuer { get; set; }
        public string? PeriodOfYear { get; set; }
        public string? Description { get; set; }

        public DoctorEntity? Doctor { get; set; }
        public SpecialtyEntity? Specialty { get; set; }
    }
}
