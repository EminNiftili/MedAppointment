namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public record ProfessionDto : ClassifierDto
    {
        public long Id { get; set; }
    }

    public record ProfessionCreateDto : ClassifierDto
    {
        public new List<CreateLocalizationDto> Name { get; set; } = new List<CreateLocalizationDto>();
        public new List<CreateLocalizationDto> Description { get; set; } = new List<CreateLocalizationDto>();
    }

    public record ProfessionUpdateDto : ClassifierDto
    {
    }
}
