namespace MedAppointment.Entities.Communication
{
    public class MeetEntity : BaseEntity
    {
        public long SenderUserId { get; set; }
        public long ReceiverUserId { get; set; }


        public UserEntity? SenderUser { get; set; }
        public UserEntity? ReceiverUser { get; set; }
    }
}
