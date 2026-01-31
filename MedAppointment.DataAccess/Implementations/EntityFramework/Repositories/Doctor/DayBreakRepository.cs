namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Doctor
{
    internal class DayBreakRepository : EfGenericRepository<DayBreakEntity>, IDayBreakRepository
    {
        public DayBreakRepository(MedicalAppointmentContext medicalAppointmentContext) : base(medicalAppointmentContext, medicalAppointmentContext.Set<DayBreakEntity>(), false)
        {
        }

        protected override IQueryable<DayBreakEntity> IncludeQuery(IQueryable<DayBreakEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
