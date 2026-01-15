namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Security
{
    internal class TraditionalUserRepository : EfGenericRepository<TraditionalUserEntity>, ITraditionalUserRepository
    {
        public TraditionalUserRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<TraditionalUserEntity>(), true)
        {
        }

        protected override IQueryable<TraditionalUserEntity> IncludeQuery(IQueryable<TraditionalUserEntity> query)
        {
            return query
                .Include(traditionalUser => traditionalUser.User)
                    .ThenInclude(user => user!.Admin)
                .Include(traditionalUser => traditionalUser.User)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(traditionalUser => traditionalUser.User)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(traditionalUser => traditionalUser.User)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(traditionalUser => traditionalUser.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Device)
                .Include(traditionalUser => traditionalUser.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Tokens)
                .Include(traditionalUser => traditionalUser.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.ReceiverUser)
                .Include(traditionalUser => traditionalUser.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(traditionalUser => traditionalUser.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.SenderUser)
                .Include(traditionalUser => traditionalUser.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User);
        }
    }
}
