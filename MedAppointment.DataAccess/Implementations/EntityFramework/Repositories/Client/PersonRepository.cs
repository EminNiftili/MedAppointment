namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client
{
    internal class PersonRepository : EfGenericRepository<PersonEntity>, IPersonRepository
    {
        public PersonRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<PersonEntity>(), true)
        {
        }

        public async Task<PersonEntity?> FindByUsernameAsync(string username, bool canIncludeAll = false)
        {
            var query = IncludeQuery(Query, canIncludeAll);
            return await query.SingleOrDefaultAsync(x => x.Email == username || x.PhoneNumber == username);

        }

        protected override IQueryable<PersonEntity> IncludeQuery(IQueryable<PersonEntity> query)
        {
            return query
                .Include(person => person.Image)
                .Include(person => person.User)
                    .ThenInclude(user => user!.Admin)
                .Include(person => person.User)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(person => person.User)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(person => person.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Device)
                .Include(person => person.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Tokens)
                .Include(person => person.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.ReceiverUser)
                .Include(person => person.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(person => person.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.SenderUser)
                .Include(person => person.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User);
        }
    }
}
