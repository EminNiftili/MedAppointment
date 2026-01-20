namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public class CurrencyDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Coefficent { get; set; }
    }

    public class CurrencyCreateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Coefficent { get; set; }
    }

    public class CurrencyUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Coefficent { get; set; }
    }
}
