namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Service
{
    internal class DayPlanRepository : EfGenericRepository<DayPlanEntity>, IDayPlanRepository
    {
        internal DayPlanRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<DayPlanEntity>(), true)
        {
        }

        protected override IQueryable<DayPlanEntity> IncludeQuery(IQueryable<DayPlanEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
