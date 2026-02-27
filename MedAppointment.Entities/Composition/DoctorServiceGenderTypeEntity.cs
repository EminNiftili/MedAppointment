namespace MedAppointment.Entities.Composition
{
    public class DoctorServiceGenderTypeEntity : BaseEntity
    {
        public long DoctorId { get; set; }
        public long GenderId { get; set; }

        public DoctorEntity? Doctor { get; set; }
        public GenderEntity? Gender { get; set; }
    }
}
