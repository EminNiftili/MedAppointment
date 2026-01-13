namespace MedAppointment.Entities.Client
{
    public class AdminEntity : BaseEntity
    {
        public long UserId { get; set; }

        public UserEntity? User { get; set; }
    }
}
