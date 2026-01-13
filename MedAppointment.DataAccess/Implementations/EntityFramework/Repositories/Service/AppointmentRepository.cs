namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Service
{
    internal class AppointmentRepository : EfGenericRepository<AppointmentEntity>, IAppointmentRepository
    {
        internal AppointmentRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<AppointmentEntity>(), true)
        {
        }

        protected override IQueryable<AppointmentEntity> IncludeQuery(IQueryable<AppointmentEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
