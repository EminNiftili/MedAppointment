namespace MedAppointment.Entities.Client
{
    public class DoctorEntity : BaseEntity
    {
        public long UserId { get; set; }
        public bool IsConfirm { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public UserEntity? User { get; set; }
        public ICollection<DoctorSpecialtyEntity> Specialties { get; set; } = new List<DoctorSpecialtyEntity>();
    }
}
