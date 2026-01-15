namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Communication
{
    internal class ChatHistoryRepository : EfGenericRepository<ChatHistoryEntity>, IChatHistoryRepository
    {
        public ChatHistoryRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<ChatHistoryEntity>(), true)
        {
        }

        protected override IQueryable<ChatHistoryEntity> IncludeQuery(IQueryable<ChatHistoryEntity> query)
        {
            return query
                .Include(history => history.User)
                    .ThenInclude(user => user!.Admin)
                .Include(history => history.User)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(history => history.User)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(history => history.User)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(history => history.Chat)
                    .ThenInclude(chat => chat!.SenderUser)
                .Include(history => history.Chat)
                    .ThenInclude(chat => chat!.ReceiverUser)
                .Include(history => history.Chat)
                    .ThenInclude(chat => chat!.Histories)
                    .ThenInclude(innerHistory => innerHistory.User);
        }
    }
}
