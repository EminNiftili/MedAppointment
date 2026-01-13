namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Service
{
    internal class PeriodPlanRepository : EfGenericRepository<PeriodPlanEntity>, IPeriodPlanRepository
    {
        internal PeriodPlanRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<PeriodPlanEntity>(), true)
        {
        }

        protected override IQueryable<PeriodPlanEntity> IncludeQuery(IQueryable<PeriodPlanEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
