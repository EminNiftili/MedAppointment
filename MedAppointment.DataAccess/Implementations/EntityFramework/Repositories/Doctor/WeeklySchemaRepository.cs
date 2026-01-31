namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Doctor
{
    internal class WeeklySchemaRepository : EfGenericRepository<WeeklySchemaEntity>, IWeeklySchemaRepository
    {
        public WeeklySchemaRepository(MedicalAppointmentContext medicalAppointmentContext) : base(medicalAppointmentContext, medicalAppointmentContext.Set<WeeklySchemaEntity>(), false)
        {
        }

        protected override IQueryable<WeeklySchemaEntity> IncludeQuery(IQueryable<WeeklySchemaEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
