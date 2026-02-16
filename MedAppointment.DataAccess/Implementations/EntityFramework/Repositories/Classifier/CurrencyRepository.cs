namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Classifier
{
    internal class CurrencyRepository : EfGenericRepository<CurrencyEntity>, ICurrencyRepository
    {
        public CurrencyRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<CurrencyEntity>(), true)
        {
        }

        protected override IQueryable<CurrencyEntity> IncludeQuery(IQueryable<CurrencyEntity> query)
        {
            return query.Include(x => x.Name)
                            .ThenInclude(r => r!.Translations)
                                .ThenInclude(t => t.Language)
                        .Include(x => x.Description)
                            .ThenInclude(r => r!.Translations)
                                .ThenInclude(t => t.Language);
        }
    }
}
