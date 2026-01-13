namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Security
{
    internal class TokenRepository : EfGenericRepository<TokenEntity>, ITokenRepository
    {
        internal TokenRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<TokenEntity>(), true)
        {
        }

        protected override IQueryable<TokenEntity> IncludeQuery(IQueryable<TokenEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
