namespace MedAppointment.Entities.Security
{
    public class TokenEntity : BaseEntity
    {
        public long SessionId { get; set; }
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiredDate { get; set; }
        public bool IsExpired { get; set; }

        public SessionEntity? Session { get; set; }
    }
}
