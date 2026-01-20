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
    }

    public record CurrencyUpdateDto : ClassifierDto
    {
        public decimal Coefficent { get; set; }
    }
}
