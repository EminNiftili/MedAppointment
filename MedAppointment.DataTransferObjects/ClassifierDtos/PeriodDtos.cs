namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public class PeriodDto : ClassifierDto
    {
        public byte PeriodTime { get; set; }
    }

    public class PeriodCreateDto : ClassifierCreateDto
    {
        public byte PeriodTime { get; set; }
    }

    public class PeriodUpdateDto : ClassifierUpdateDto
    {
        public byte PeriodTime { get; set; }
    }
}
