namespace MedAppointment.DataAccess.Repositories.Classifier
{
    internal class PeriodRepository : EfGenericRepository<PeriodEntity>, IPeriodRepository
    {
        public PeriodRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<PeriodEntity>(), false)
        {
        }

        protected override IQueryable<PeriodEntity> IncludeQuery(IQueryable<PeriodEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
