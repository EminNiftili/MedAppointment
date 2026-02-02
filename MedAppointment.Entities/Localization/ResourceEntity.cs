namespace MedAppointment.Entities.Localization
{
    public class ResourceEntity : BaseEntity
    {
        public string Key { get; set; } = null!;
        public ICollection<TranslationEntity> Translations { get; set; } = new List<TranslationEntity>();
    }
}
