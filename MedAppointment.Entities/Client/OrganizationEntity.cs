namespace MedAppointment.Entities.Client
{
    public class OrganizationEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? EmailCode { get; set; }
        public ICollection<OrganizationUserEntity> OrganizationUsers { get; set; } = new List<OrganizationUserEntity>();
    }
}
