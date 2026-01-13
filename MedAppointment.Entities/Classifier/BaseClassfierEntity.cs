namespace MedAppointment.Entities.Classifier
{
    public abstract class BaseClassfierEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
