namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Communication
{
    internal class ChatHistoryRepository : EfGenericRepository<ChatHistoryEntity>, IChatHistoryRepository
    {
        internal ChatHistoryRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<ChatHistoryEntity>(), true)
        {
        }

        protected override IQueryable<ChatHistoryEntity> IncludeQuery(IQueryable<ChatHistoryEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
