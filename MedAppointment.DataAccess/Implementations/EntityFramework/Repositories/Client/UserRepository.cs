namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client
{
    internal class UserRepository : EfGenericRepository<UserEntity>, IUserRepository
    {
        public UserRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<UserEntity>(), true)
        {
        }

        protected override IQueryable<UserEntity> IncludeQuery(IQueryable<UserEntity> query)
        {
            return query
                .Include(user => user.Admin)
                .Include(user => user.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(user => user.Person)
                    .ThenInclude(person => person!.Image)
                .Include(user => user.TraditionalUser)
                .Include(user => user.Sessions)
                    .ThenInclude(session => session.Device)
                .Include(user => user.Sessions)
                    .ThenInclude(session => session.Tokens)
                .Include(user => user.SentChats)
                    .ThenInclude(chat => chat.ReceiverUser)
                .Include(user => user.SentChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(user => user.ReceiverChats)
                    .ThenInclude(chat => chat.SenderUser)
                .Include(user => user.ReceiverChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User);
        }
    }
}
