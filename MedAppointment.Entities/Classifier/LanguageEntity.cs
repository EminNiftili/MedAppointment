namespace MedAppointment.Entities.Classifier
{
    public class LanguageEntity : BaseEntity
    {
        public bool IsDefault { get; set; }
        public string Name { get; set; } = null!;
    }
}
