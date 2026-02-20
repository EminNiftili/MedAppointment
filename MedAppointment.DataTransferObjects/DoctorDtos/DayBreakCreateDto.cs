namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record DayBreakCreateDto
    {
        public string? Name { get; set; }
        public bool IsVisible { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
