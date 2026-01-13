namespace MedAppointment.Entities.Communication
{
    public class ChatEntity : BaseEntity
    {
        public long SenderUserId { get; set; }
        public long ReceiverUserId { get; set; }


        public UserEntity? SenderUser { get; set; }
        public UserEntity? ReceiverUser { get; set; }
        public ICollection<ChatHistoryEntity> Histories { get; set; } = new List<ChatHistoryEntity>();

    }
}
