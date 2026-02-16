namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public record CurrencyDto : ClassifierDto
    {
        public long Id { get; set; }
        public decimal Coefficent { get; set; }
    }

    public record CurrencyCreateDto : ClassifierDto
    {
        public decimal Coefficent { get; set; }
        public new List<CreateLocalizationDto> Name { get; set; } = new List<CreateLocalizationDto>();
        public new List<CreateLocalizationDto> Description { get; set; } = new List<CreateLocalizationDto>();
    }

    public record CurrencyUpdateDto : ClassifierDto
    {
        public decimal Coefficent { get; set; }
    }
}
