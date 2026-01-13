namespace MedAppointment.Entities.Communication
{
    public class ChatHistoryEntity : BaseEntity
    {
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public string Message { get; set; } = null!;
        public bool IsSeen { get; set; } = false;
        public bool IsSent { get; set; } = false;


        public UserEntity? User { get; set; }
        public ChatEntity? Chat { get; set; }
    }
}
