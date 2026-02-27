namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Classifier
{
    internal class GenderRepository : EfGenericRepository<GenderEntity>, IGenderRepository
    {
        public GenderRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<GenderEntity>(), false)
        {
        }

        protected override IQueryable<GenderEntity> IncludeQuery(IQueryable<GenderEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
