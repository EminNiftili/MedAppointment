namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client
{
    internal class OrganizationRepository : EfGenericRepository<OrganizationEntity>, IOrganizationRepository
    {
        public OrganizationRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<OrganizationEntity>(), true)
        {
        }

        protected override IQueryable<OrganizationEntity> IncludeQuery(IQueryable<OrganizationEntity> query)
        {
            return query
                .Include(organization => organization.OrganizationUsers)
                    .ThenInclude(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.Admin)
                .Include(organization => organization.OrganizationUsers)
                    .ThenInclude(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(organization => organization.OrganizationUsers)
                    .ThenInclude(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(organization => organization.OrganizationUsers)
                    .ThenInclude(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(organization => organization.OrganizationUsers)
                    .ThenInclude(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Device)
                .Include(organization => organization.OrganizationUsers)
                    .ThenInclude(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Tokens)
                .Include(organization => organization.OrganizationUsers)
                    .ThenInclude(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.ReceiverUser)
                .Include(organization => organization.OrganizationUsers)
                    .ThenInclude(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(organization => organization.OrganizationUsers)
                    .ThenInclude(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.SenderUser)
                .Include(organization => organization.OrganizationUsers)
                    .ThenInclude(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User);
        }
    }
}
