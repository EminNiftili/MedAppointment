namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Security
{
    internal class TraditionalUserRepository : EfGenericRepository<TraditionalUserEntity>, ITraditionalUserRepository
    {
        internal TraditionalUserRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<TraditionalUserEntity>(), true)
        {
        }

        protected override IQueryable<TraditionalUserEntity> IncludeQuery(IQueryable<TraditionalUserEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
