namespace MedAppointment.Entities.Client
{
    public class UserEntity : BaseEntity
    {
        public long PersonId { get; set; }
        /// <summary>
        /// <list type="table">
        /// <item>0 -> Traditional</item>
        /// <item>1 -> Google</item>
        /// <item>2 -> Facebook</item>
        /// <item>3 -> Apple</item>
        /// </list>
        /// </summary>
        public byte Provider { get; set; }

        public AdminEntity? Admin { get; set; }
        public DoctorEntity? Doctor { get; set; }
        public PersonEntity? Person { get; set; }
        public TraditionalUserEntity? TraditionalUser { get; set; }
        public ICollection<SessionEntity> Sessions { get; set; } = new List<SessionEntity>();
        public ICollection<ChatEntity> SentChats { get; set; } = new List<ChatEntity>();
        public ICollection<ChatEntity> ReceiverChats { get; set; } = new List<ChatEntity>();
    }
}
