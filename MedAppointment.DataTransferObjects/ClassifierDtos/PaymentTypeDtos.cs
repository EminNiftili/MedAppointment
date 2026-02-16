namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public record PaymentTypeDto : ClassifierDto
    {
        public long Id { get; set; }
    }

    public record PaymentTypeCreateDto : ClassifierDto
    {
        public new List<CreateLocalizationDto> Name { get; set; } = new List<CreateLocalizationDto>();
        public new List<CreateLocalizationDto> Description { get; set; } = new List<CreateLocalizationDto>();
    }

    public record PaymentTypeUpdateDto : ClassifierDto
    {
    }
}
