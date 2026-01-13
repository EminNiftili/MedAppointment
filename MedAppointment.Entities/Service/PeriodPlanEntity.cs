namespace MedAppointment.Entities.Service
{
    public class PeriodPlanEntity : BaseEntity
    {
        public long DayPlanId { get; set; }
        public TimeSpan PeriodStart { get; set; }
        public TimeSpan PeriodStop { get; set; }
        public bool IsOnlineService { get; set; }
        public bool IsOnSiteService { get; set; }

        public decimal PricePerPeriod { get; set; }  
        public long CurrencyId { get; set; }
        public bool IsBusy { get; set; }

        public DayPlanEntity? DayPlan { get; set; }
        public CurrencyEntity? Currency { get; set; }
    }
}
