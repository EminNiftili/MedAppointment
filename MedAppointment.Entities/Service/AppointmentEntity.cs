namespace MedAppointment.Entities.Service
{
    public class AppointmentEntity : BaseEntity
    {
        public long PeriodPlanId { get; set; }
        /// <summary>
        /// <list type="bullet">
        /// <item>0 -> OnSite</item>
        /// <item>1 -> Online</item>
        /// </list>
        /// </summary>
        public byte SelectedServiceType { get; set; }
        public long PaymentId { get; set; }

        public PaymentEntity? Payment { get; set; }
        public PeriodPlanEntity? PeriodPlan { get; set; }
    }
}
