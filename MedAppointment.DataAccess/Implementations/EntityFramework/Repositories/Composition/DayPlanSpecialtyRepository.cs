namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Composition
{
    internal class DayPlanSpecialtyRepository : EfGenericRepository<DayPlanSpecialtyEntity>, IDayPlanSpecialtyRepository
    {
        public DayPlanSpecialtyRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<DayPlanSpecialtyEntity>(), true)
        {
        }

        protected override IQueryable<DayPlanSpecialtyEntity> IncludeQuery(IQueryable<DayPlanSpecialtyEntity> query)
        {
            return query
                .Include(x => x.DayPlan)
                .Include(x => x.Specialty);
        }
    }
}
