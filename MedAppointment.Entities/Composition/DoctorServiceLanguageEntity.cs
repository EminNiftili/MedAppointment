namespace MedAppointment.Entities.Composition
{
    public class DoctorServiceLanguageEntity : BaseEntity
    {
        public long DoctorId { get; set; }
        public long LanguageId { get; set; }

        public DoctorEntity? Doctor { get; set; }
        public LanguageEntity? Language { get; set; }
    }
}
