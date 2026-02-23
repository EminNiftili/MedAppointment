namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record DoctorCalendarPeriodSlotDto
    {
        public long PeriodPlanId { get; init; }
        public TimeSpan PeriodStart { get; init; }
        public TimeSpan PeriodStop { get; init; }
        public decimal PricePerPeriod { get; init; }
        public long CurrencyId { get; init; }
        public string CurrencyKey { get; init; } = null!;
        public long PeriodId { get; init; }
        public byte PeriodTimeMinutes { get; init; }
        public long SpecialtyId { get; init; }
        public bool IsBusy { get; init; }
        public bool IsOnlineService { get; init; }
        public bool IsOnSiteService { get; init; }
    }
}
