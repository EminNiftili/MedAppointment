namespace MedAppointment.Entities.Composition
{
    public class DoctorSpecialtyEntity : BaseEntity
    {
        public bool IsConfirm { get; set; }
        public long DoctorId { get; set; }
        public long SpecialtyId { get; set; }

        public DoctorEntity? Doctor { get; set; }
        public SpecialtyEntity? Specialty { get; set; }
    }
}
