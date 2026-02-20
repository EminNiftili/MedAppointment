namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Doctor
{
    internal class WeeklySchemaRepository : EfGenericRepository<WeeklySchemaEntity>, IWeeklySchemaRepository
    {
        public WeeklySchemaRepository(MedicalAppointmentContext medicalAppointmentContext) : base(medicalAppointmentContext, medicalAppointmentContext.Set<WeeklySchemaEntity>(), true)
        {
        }

        protected override IQueryable<WeeklySchemaEntity> IncludeQuery(IQueryable<WeeklySchemaEntity> query)
        {
            return query.Include(x => x.DayPlans)
                            .ThenInclude(x => x.Period)
                        .Include(x => x.DayPlans)
                            .ThenInclude(d => d.DayBreaks)
                        .Include(x => x.DayPlans)
                            .ThenInclude(d => d.PlanPaddingType)
                        .Include(x => x.DayPlans)
                            .ThenInclude(d => d.Specialty)
                        .Include(x => x.Doctor)
                            .ThenInclude(x => x!.User);
        }
    }
}
