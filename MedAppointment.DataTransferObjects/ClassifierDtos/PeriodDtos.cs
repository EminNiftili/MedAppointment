namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public record PeriodDto : ClassifierDto
    {
        public long Id { get; set; }
        public byte PeriodTime { get; set; }
    }

    public record PeriodCreateDto : ClassifierDto
    {
        public byte PeriodTime { get; set; }
        public new List<CreateLocalizationDto> Name { get; set; } = new List<CreateLocalizationDto>();
        public new List<CreateLocalizationDto> Description { get; set; } = new List<CreateLocalizationDto>();
    }

    public record PeriodUpdateDto : ClassifierDto
    {
        public byte PeriodTime { get; set; }
    }
}
