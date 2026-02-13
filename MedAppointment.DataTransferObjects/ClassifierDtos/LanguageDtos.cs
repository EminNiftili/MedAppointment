namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public record LanguageDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public record LanguageCreateDto
    {
        public string Name { get; set; } = null!;
        public bool IsDefault { get; set; }
    }

    public record LanguageUpdateDto
    {
        public string Name { get; set; } = null!;
        public bool IsDefault { get; set; }
    }
}
