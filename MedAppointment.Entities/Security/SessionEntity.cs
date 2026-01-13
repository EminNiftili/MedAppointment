namespace MedAppointment.Entities.Security
{
    public class SessionEntity : BaseEntity
    {
        public long UserId { get; set; }
        public long DeviceId { get; set; }

        public UserEntity? User { get; set; }
        public DeviceEntity? Device { get; set; }
        public ICollection<TokenEntity> Tokens { get; set; } = new List<TokenEntity>();
    }
}
