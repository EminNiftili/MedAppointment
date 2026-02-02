namespace MedAppointment.Entities.Classifier
{
    public abstract class BaseClassfierEntity : BaseEntity
    {
        public string Key { get; set; } = null!;
        public long NameTextId { get; set; }
        public long DescriptionTextId { get; set; }

        public ResourceEntity? Name { get; set; }
        public ResourceEntity? Description { get; set; }
    }
}
