namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Classifier
{
    internal class ProfessionRepository : EfGenericRepository<ProfessionEntity>, IProfessionRepository
    {
        public ProfessionRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<ProfessionEntity>(), false)
        {
        }

        protected override IQueryable<ProfessionEntity> IncludeQuery(IQueryable<ProfessionEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
