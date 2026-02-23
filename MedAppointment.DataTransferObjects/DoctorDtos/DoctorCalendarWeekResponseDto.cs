namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record DoctorCalendarWeekResponseDto
    {
        public long DoctorId { get; init; }
        public DateTime WeekStartDate { get; init; }
        public DateTime WeekEndDate { get; init; }
        public IReadOnlyCollection<DoctorCalendarDayDto> Days { get; init; } = Array.Empty<DoctorCalendarDayDto>();
    }
}
