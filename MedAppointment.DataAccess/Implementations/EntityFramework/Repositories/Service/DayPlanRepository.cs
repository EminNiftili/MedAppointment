namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Service
{
    internal class DayPlanRepository : EfGenericRepository<DayPlanEntity>, IDayPlanRepository
    {
        public DayPlanRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<DayPlanEntity>(), true)
        {
        }

        protected override IQueryable<DayPlanEntity> IncludeQuery(IQueryable<DayPlanEntity> query)
        {
            return query
                .Include(dayPlan => dayPlan.Doctor)
                    .ThenInclude(doctor => doctor!.User)
                .Include(dayPlan => dayPlan.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(dayPlan => dayPlan.Specialty)
                .Include(dayPlan => dayPlan.Period);
        }
    }
}
