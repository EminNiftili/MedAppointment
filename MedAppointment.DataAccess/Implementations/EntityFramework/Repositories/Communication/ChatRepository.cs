namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Communication
{
    internal class ChatRepository : EfGenericRepository<ChatEntity>, IChatRepository
    {
        public ChatRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<ChatEntity>(), true)
        {
        }

        protected override IQueryable<ChatEntity> IncludeQuery(IQueryable<ChatEntity> query)
        {
            return query
                .Include(chat => chat.SenderUser)
                    .ThenInclude(user => user!.Admin)
                .Include(chat => chat.SenderUser)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(chat => chat.SenderUser)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(chat => chat.SenderUser)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(chat => chat.ReceiverUser)
                    .ThenInclude(user => user!.Admin)
                .Include(chat => chat.ReceiverUser)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(chat => chat.ReceiverUser)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(chat => chat.ReceiverUser)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image);
        }
    }
}
