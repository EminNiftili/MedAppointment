namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Service
{
    internal class PeriodPlanRepository : EfGenericRepository<PeriodPlanEntity>, IPeriodPlanRepository
    {
        public PeriodPlanRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<PeriodPlanEntity>(), true)
        {
        }

        protected override IQueryable<PeriodPlanEntity> IncludeQuery(IQueryable<PeriodPlanEntity> query)
        {
            return query
                .Include(periodPlan => periodPlan.DayPlan)
                    .ThenInclude(dayPlan => dayPlan!.Doctor)
                    .ThenInclude(doctor => doctor!.User)
                .Include(periodPlan => periodPlan.DayPlan)
                    .ThenInclude(dayPlan => dayPlan!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(periodPlan => periodPlan.DayPlan)
                    .ThenInclude(dayPlan => dayPlan!.Specialty)
                .Include(periodPlan => periodPlan.DayPlan)
                    .ThenInclude(dayPlan => dayPlan!.Period)
                .Include(periodPlan => periodPlan.Currency);
        }
    }
}
