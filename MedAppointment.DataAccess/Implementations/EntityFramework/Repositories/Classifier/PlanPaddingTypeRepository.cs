namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Classifier
{
    internal class PlanPaddingTypeRepository : EfGenericRepository<PlanPaddingTypeEntity>, IPlanPaddingTypeRepository
    {
        public PlanPaddingTypeRepository(MedicalAppointmentContext medicalAppointmentContext) : base(medicalAppointmentContext, medicalAppointmentContext.PlanPaddingTypes, false)
        {
        }

        protected override IQueryable<PlanPaddingTypeEntity> IncludeQuery(IQueryable<PlanPaddingTypeEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
