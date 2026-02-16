namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Classifier
{
    internal class PlanPaddingTypeRepository : EfGenericRepository<PlanPaddingTypeEntity>, IPlanPaddingTypeRepository
    {
        public PlanPaddingTypeRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.PlanPaddingTypes, true)
        {
        }

        protected override IQueryable<PlanPaddingTypeEntity> IncludeQuery(IQueryable<PlanPaddingTypeEntity> query)
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
