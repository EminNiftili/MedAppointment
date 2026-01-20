namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public class CurrencyDto : ClassifierDto
    {
        public decimal Coefficent { get; set; }
    }

    public class CurrencyCreateDto : ClassifierCreateDto
    {
        public decimal Coefficent { get; set; }
    }

    public class CurrencyUpdateDto : ClassifierUpdateDto
    {
        public decimal Coefficent { get; set; }
    }
}
