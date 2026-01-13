namespace MedAppointment.Entities.Composition
{
    public class OrganizationUserEntity : BaseEntity
    {
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public bool IsAdmin { get; set; }

        public OrganizationEntity? Organization { get; set; }
        public UserEntity? User { get; set; }
    }
}
