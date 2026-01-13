namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client
{
    internal class UserRepository : EfGenericRepository<UserEntity>, IUserRepository
    {
        internal UserRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<UserEntity>(), true)
        {
        }

        protected override IQueryable<UserEntity> IncludeQuery(IQueryable<UserEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
