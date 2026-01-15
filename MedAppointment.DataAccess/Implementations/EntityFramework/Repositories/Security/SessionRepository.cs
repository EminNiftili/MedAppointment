namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Security
{
    internal class SessionRepository : EfGenericRepository<SessionEntity>, ISessionRepository
    {
        public SessionRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<SessionEntity>(), true)
        {
        }

        protected override IQueryable<SessionEntity> IncludeQuery(IQueryable<SessionEntity> query)
        {
            return query
                .Include(session => session.User)
                    .ThenInclude(user => user!.Admin)
                .Include(session => session.User)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(session => session.User)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(session => session.User)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(session => session.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(userSession => userSession.Device)
                .Include(session => session.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(userSession => userSession.Tokens)
                .Include(session => session.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.ReceiverUser)
                .Include(session => session.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(session => session.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.SenderUser)
                .Include(session => session.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(session => session.Device)
                .Include(session => session.Tokens);
        }
    }
}
