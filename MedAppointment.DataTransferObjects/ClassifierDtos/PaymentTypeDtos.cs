namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public class PaymentTypeDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class PaymentTypeCreateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class PaymentTypeUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
