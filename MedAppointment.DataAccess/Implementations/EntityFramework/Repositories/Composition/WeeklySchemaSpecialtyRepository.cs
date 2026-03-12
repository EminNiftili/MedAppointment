namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Composition
{
    internal class WeeklySchemaSpecialtyRepository : EfGenericRepository<WeeklySchemaSpecialtyEntity>, IWeeklySchemaSpecialtyRepository
    {
        public WeeklySchemaSpecialtyRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<WeeklySchemaSpecialtyEntity>(), true)
        {
        }

        protected override IQueryable<WeeklySchemaSpecialtyEntity> IncludeQuery(IQueryable<WeeklySchemaSpecialtyEntity> query)
        {
            return query
                .Include(x => x.WeeklySchema)
                .Include(x => x.Specialty);
        }
    }
}
