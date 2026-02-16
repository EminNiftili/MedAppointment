namespace MedAppointment.DataAccess.Repositories.Classifier
{
    internal class SpecialtyRepository : EfGenericRepository<SpecialtyEntity>, ISpecialtyRepository
    {
        public SpecialtyRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<SpecialtyEntity>(), true)
        {
        }

        protected override IQueryable<SpecialtyEntity> IncludeQuery(IQueryable<SpecialtyEntity> query)
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
