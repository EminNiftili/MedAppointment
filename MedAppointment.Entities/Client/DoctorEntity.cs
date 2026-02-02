namespace MedAppointment.Entities.Client
{
    public class DoctorEntity : BaseEntity
    {
        public long UserId { get; set; }
        public bool IsConfirm { get; set; }
        public long TitleTextId { get; set; }
        public long DescriptionTextId { get; set; }

        public ResourceEntity? Title {  get; set; }
        public ResourceEntity? Description { get; set; }
        public UserEntity? User { get; set; }
        public ICollection<DoctorSpecialtyEntity> Specialties { get; set; } = new List<DoctorSpecialtyEntity>();
    }
}
