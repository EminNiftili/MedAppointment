namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Classifier
{
    internal class CurrencyRepository : EfGenericRepository<CurrencyEntity>, ICurrencyRepository
    {
        public CurrencyRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<CurrencyEntity>(), false)
        {
        }

        protected override IQueryable<CurrencyEntity> IncludeQuery(IQueryable<CurrencyEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
