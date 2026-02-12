namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Localization
{
    internal class TranslationRepository : EfGenericRepository<TranslationEntity>, ITranslationRepository
    {
        public TranslationRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<TranslationEntity>(), true)
        {
        }

        protected override IQueryable<TranslationEntity> IncludeQuery(IQueryable<TranslationEntity> query)
        {
            return query;
        }
    }
}
