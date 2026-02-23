namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record EditDayPlanDto
    {
        public long DayPlanId { get; init; }
        public long DoctorId { get; init; }
        public long SpecialtyId { get; init; }
        public bool IsClosed { get; init; }
    }
}
