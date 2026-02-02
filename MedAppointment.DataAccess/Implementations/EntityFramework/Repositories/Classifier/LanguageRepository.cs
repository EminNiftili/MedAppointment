namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Classifier
{
    internal class LanguageRepository : EfGenericRepository<LanguageEntity>, ILanguageRepository
    {
        public LanguageRepository(MedicalAppointmentContext medicalAppointmentContext) : base(medicalAppointmentContext, medicalAppointmentContext.Set<LanguageEntity>(), false)
        {
        }

        protected override IQueryable<LanguageEntity> IncludeQuery(IQueryable<LanguageEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
