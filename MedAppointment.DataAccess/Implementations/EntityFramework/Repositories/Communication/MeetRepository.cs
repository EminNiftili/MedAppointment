namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Communication
{
    internal class MeetRepository : EfGenericRepository<MeetEntity>, IMeetRepository
    {
        internal MeetRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<MeetEntity>(), true)
        {
        }

        protected override IQueryable<MeetEntity> IncludeQuery(IQueryable<MeetEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
