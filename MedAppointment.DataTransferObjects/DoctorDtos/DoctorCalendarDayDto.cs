namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record DoctorCalendarDayDto
    {
        public DateTime Date { get; init; }
        public int DayOfWeek { get; init; }
        public bool IsClosed { get; init; }
        public IReadOnlyCollection<DoctorCalendarPeriodSlotDto> Periods { get; init; } = Array.Empty<DoctorCalendarPeriodSlotDto>();
    }
}
