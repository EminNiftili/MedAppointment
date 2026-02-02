namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Localization
{
    internal class TranslationRepository : EfGenericRepository<TranslationEntity>, ITranslationRepository
    {
        public TranslationRepository(MedicalAppointmentContext medicalAppointmentContext) : base(medicalAppointmentContext, medicalAppointmentContext.Set<TranslationEntity>(), false)
        {
        }

        protected override IQueryable<TranslationEntity> IncludeQuery(IQueryable<TranslationEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
