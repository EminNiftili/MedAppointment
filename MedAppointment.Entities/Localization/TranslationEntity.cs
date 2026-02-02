namespace MedAppointment.Entities.Localization
{
    public class TranslationEntity : BaseEntity
    {
        public long ResourceId { get; set; }
        public long LanguageId { get; set; }
        public string Text { get; set; } = null!;

        public ResourceEntity? Resource { get; set; }
        public LanguageEntity? Language { get; set; }
    }
}
