namespace MedAppointment.DataTransferObjects.LocalizationDtos
{
    public record CreateLocalizationDto
    {
        public long LanguageId { get; set; }
        public string Text { get; set; } = null!;
    }
    public record LocalizationDto : CreateLocalizationDto
    {
        public string Key { get; set; } = null!;
    }
}
