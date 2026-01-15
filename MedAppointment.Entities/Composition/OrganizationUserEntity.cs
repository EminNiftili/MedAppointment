namespace MedAppointment.Entities.Composition
{
    public class OrganizationUserEntity : BaseEntity
    {
        public long UserId { get; set; }
        public long OrganizationId { get; set; }
        public bool IsAdmin { get; set; }

        public OrganizationEntity? Organization { get; set; }
        public UserEntity? User { get; set; }
    }
}
