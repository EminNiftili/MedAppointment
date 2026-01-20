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
    }

    public record PeriodUpdateDto : ClassifierDto
    {
        public byte PeriodTime { get; set; }
    }
}
