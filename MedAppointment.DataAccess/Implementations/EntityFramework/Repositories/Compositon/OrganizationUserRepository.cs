using MedAppointment.DataAccess.Repositories.Composition;

namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Compositon
{
    internal class OrganizationUserRepository : EfGenericRepository<OrganizationUserEntity>, IOrganizationUserRepository
    {
        internal OrganizationUserRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<OrganizationUserEntity>(), true)
        {
        }

        protected override IQueryable<OrganizationUserEntity> IncludeQuery(IQueryable<OrganizationUserEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
