namespace MedAppointment.DataAccess.Repositories.Classifier
{
    internal class PeriodRepository : EfGenericRepository<PeriodEntity>, IPeriodRepository
    {
        public PeriodRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<PeriodEntity>(), true)
        {
        }

        protected override IQueryable<PeriodEntity> IncludeQuery(IQueryable<PeriodEntity> query)
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
