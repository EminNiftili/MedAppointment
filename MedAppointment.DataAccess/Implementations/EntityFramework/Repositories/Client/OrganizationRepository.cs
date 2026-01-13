namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client
{
    internal class OrganizationRepository : EfGenericRepository<OrganizationEntity>, IOrganizationRepository
    {
        internal OrganizationRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<OrganizationEntity>(), true)
        {
        }

        protected override IQueryable<OrganizationEntity> IncludeQuery(IQueryable<OrganizationEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
