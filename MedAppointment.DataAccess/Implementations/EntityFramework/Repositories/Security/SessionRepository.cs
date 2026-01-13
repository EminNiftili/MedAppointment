namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Security
{
    internal class SessionRepository : EfGenericRepository<SessionEntity>, ISessionRepository
    {
        internal SessionRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<SessionEntity>(), true)
        {
        }

        protected override IQueryable<SessionEntity> IncludeQuery(IQueryable<SessionEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
