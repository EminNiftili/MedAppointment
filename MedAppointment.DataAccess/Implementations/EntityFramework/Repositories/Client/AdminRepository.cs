namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client
{
    internal class AdminRepository : EfGenericRepository<AdminEntity>, IAdminRepository
    {
        internal AdminRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<AdminEntity>(), true)
        {
        }

        protected override IQueryable<AdminEntity> IncludeQuery(IQueryable<AdminEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
