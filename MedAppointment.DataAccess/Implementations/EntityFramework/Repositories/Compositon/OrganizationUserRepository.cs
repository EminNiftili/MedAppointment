namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Compositon
{
    internal class OrganizationUserRepository : EfGenericRepository<OrganizationUserEntity>, IOrganizationUserRepository
    {
        public OrganizationUserRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<OrganizationUserEntity>(), true)
        {
        }

        protected override IQueryable<OrganizationUserEntity> IncludeQuery(IQueryable<OrganizationUserEntity> query)
        {
            return query
                .Include(organizationUser => organizationUser.Organization)
                    .ThenInclude(organization => organization!.OrganizationUsers)
                    .ThenInclude(innerOrganizationUser => innerOrganizationUser.User)
                .Include(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.Admin)
                .Include(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Device)
                .Include(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Tokens)
                .Include(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.ReceiverUser)
                .Include(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.SenderUser)
                .Include(organizationUser => organizationUser.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User);
        }
    }
}
