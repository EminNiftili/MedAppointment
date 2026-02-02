namespace MedAppointment.DataTransferObjects.LocalizationDtos
{
    public class LocalizationDto
    {
        public string Key { get; set; } = null!;
        public long LanguageId { get; set; }
        public string Text { get; set; } = null!;
    }
}
