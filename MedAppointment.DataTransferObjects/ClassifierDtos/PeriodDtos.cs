namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public class PeriodDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public byte PeriodTime { get; set; }
    }

    public class PeriodCreateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public byte PeriodTime { get; set; }
    }

    public class PeriodUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public byte PeriodTime { get; set; }
    }
}
