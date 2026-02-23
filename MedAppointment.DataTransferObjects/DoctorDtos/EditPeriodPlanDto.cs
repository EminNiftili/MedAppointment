namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record EditPeriodPlanDto
    {
        public long PeriodPlanId { get; init; }
        public long DoctorId { get; init; }
        public TimeSpan PeriodStart { get; init; }
        public TimeSpan PeriodStop { get; init; }
        public bool IsOnlineService { get; init; }
        public bool IsOnSiteService { get; init; }
        public decimal PricePerPeriod { get; init; }
        public long CurrencyId { get; init; }
        public bool IsBusy { get; init; }
    }
}
