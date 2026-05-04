namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Classifier
{
    internal class ProfessionRepository : EfGenericRepository<ProfessionEntity>, IProfessionRepository
    {
        public ProfessionRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<ProfessionEntity>(), true)
        {
        }

        protected override IQueryable<ProfessionEntity> IncludeQuery(IQueryable<ProfessionEntity> query)
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
