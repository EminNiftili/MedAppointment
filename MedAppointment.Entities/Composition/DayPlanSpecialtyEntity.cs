namespace MedAppointment.Entities.Composition
{
    public class DayPlanSpecialtyEntity : BaseEntity
    {
        public long DayPlanId { get; set; }
        public long SpecialtyId { get; set; }

        public DayPlanEntity? DayPlan { get; set; }
        public SpecialtyEntity? Specialty { get; set; }
    }
}
