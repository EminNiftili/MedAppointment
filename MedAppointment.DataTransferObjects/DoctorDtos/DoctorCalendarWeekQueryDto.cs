namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record DoctorCalendarWeekQueryDto
    {
        public long DoctorId { get; init; }
        public DateTime WeekStartDate { get; init; }
    }
}
