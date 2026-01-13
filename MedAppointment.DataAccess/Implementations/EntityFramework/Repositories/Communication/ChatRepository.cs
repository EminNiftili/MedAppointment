namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Communication
{
    internal class ChatRepository : EfGenericRepository<ChatEntity>, IChatRepository
    {
        internal ChatRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<ChatEntity>(), true)
        {
        }

        protected override IQueryable<ChatEntity> IncludeQuery(IQueryable<ChatEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
