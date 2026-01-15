namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client
{
    internal class AdminRepository : EfGenericRepository<AdminEntity>, IAdminRepository
    {
        public AdminRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<AdminEntity>(), true)
        {
        }

        protected override IQueryable<AdminEntity> IncludeQuery(IQueryable<AdminEntity> query)
        {
            return query
                .Include(admin => admin.User)
                    .ThenInclude(user => user!.Admin)
                .Include(admin => admin.User)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(admin => admin.User)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(admin => admin.User)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(admin => admin.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Device)
                .Include(admin => admin.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Tokens)
                .Include(admin => admin.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.ReceiverUser)
                .Include(admin => admin.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(admin => admin.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.SenderUser)
                .Include(admin => admin.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User);
        }
    }
}
