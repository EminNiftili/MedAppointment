namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Security
{
    internal class TokenRepository : EfGenericRepository<TokenEntity>, ITokenRepository
    {
        public TokenRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<TokenEntity>(), true)
        {
        }

        protected override IQueryable<TokenEntity> IncludeQuery(IQueryable<TokenEntity> query)
        {
            return query
                .Include(token => token.Session)
                    .ThenInclude(session => session!.User)
                    .ThenInclude(user => user!.Admin)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.User)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.User)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.User)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(userSession => userSession.Device)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(userSession => userSession.Tokens)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.ReceiverUser)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.SenderUser)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat!.Histories)
                    .ThenInclude(history => history.User)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.Device)
                .Include(token => token.Session)
                    .ThenInclude(session => session!.Tokens);
        }
    }
}
