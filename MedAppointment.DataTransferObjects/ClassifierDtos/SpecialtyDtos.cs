namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public record SpecialtyDto : ClassifierDto
    {
        public long Id { get; set; }
    }

    public record SpecialtyCreateDto : ClassifierDto
    {
        public new List<CreateLocalizationDto> Name { get; set; } = new List<CreateLocalizationDto>();
        public new List<CreateLocalizationDto> Description { get; set; } = new List<CreateLocalizationDto>();
    }

    public record SpecialtyUpdateDto : ClassifierDto
    {
    }
}
