namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public record PaymentTypeDto : ClassifierDto
    {
        public long Id { get; set; }
    }

    public record PaymentTypeCreateDto : ClassifierDto
    {
    }

    public record PaymentTypeUpdateDto : ClassifierDto
    {
    }
}
