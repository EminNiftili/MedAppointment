namespace MedAppointment.Entities.Client
{
    public class DoctorEntity : BaseEntity
    {
        public long UserId { get; set; }
        public long ProfessionId { get; set; }
        public bool IsConfirm { get; set; }
        public long TitleTextId { get; set; }
        public long DescriptionTextId { get; set; }
        public string? PresentationVideoUrl { get; set; }

        public ProfessionEntity? Profession { get; set; }
        public ResourceEntity? Title {  get; set; }
        public ResourceEntity? Description { get; set; }
        public UserEntity? User { get; set; }
        public ICollection<DoctorSpecialtyEntity> Specialties { get; set; } = new List<DoctorSpecialtyEntity>();
        public ICollection<DoctorServiceGenderTypeEntity> ServiceGenderTypes { get; set; } = new List<DoctorServiceGenderTypeEntity>();
        public ICollection<DoctorServiceLanguageEntity> ServiceLanguages { get; set; } = new List<DoctorServiceLanguageEntity>();
    }
}
