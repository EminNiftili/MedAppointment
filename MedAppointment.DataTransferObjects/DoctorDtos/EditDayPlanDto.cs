namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record EditDayPlanDto
    {
        public long DayPlanId { get; init; }
        public long DoctorId { get; init; }
        public IEnumerable<long> SpecialtyIds { get; init; } = new List<long>();
        public bool IsClosed { get; init; }
    }
}
