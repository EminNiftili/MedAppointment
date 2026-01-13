namespace MedAppointment.Entities.Security
{
    public class TraditionalUserEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string PasswordHash { get; set; } = null!;
        
        public UserEntity? User { get; set; }
    }
}
