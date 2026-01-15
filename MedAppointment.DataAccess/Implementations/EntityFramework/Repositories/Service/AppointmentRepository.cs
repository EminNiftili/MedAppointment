namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Service
{
    internal class AppointmentRepository : EfGenericRepository<AppointmentEntity>, IAppointmentRepository
    {
        public AppointmentRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<AppointmentEntity>(), true)
        {
        }

        protected override IQueryable<AppointmentEntity> IncludeQuery(IQueryable<AppointmentEntity> query)
        {
            return query
                .Include(appointment => appointment.Payment)
                    .ThenInclude(payment => payment!.PaymentType)
                .Include(appointment => appointment.PeriodPlan)
                    .ThenInclude(periodPlan => periodPlan!.DayPlan)
                    .ThenInclude(dayPlan => dayPlan!.Doctor)
                    .ThenInclude(doctor => doctor!.User)
                .Include(appointment => appointment.PeriodPlan)
                    .ThenInclude(periodPlan => periodPlan!.DayPlan)
                    .ThenInclude(dayPlan => dayPlan!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(appointment => appointment.PeriodPlan)
                    .ThenInclude(periodPlan => periodPlan!.DayPlan)
                    .ThenInclude(dayPlan => dayPlan!.Specialty)
                .Include(appointment => appointment.PeriodPlan)
                    .ThenInclude(periodPlan => periodPlan!.DayPlan)
                    .ThenInclude(dayPlan => dayPlan!.Period)
                .Include(appointment => appointment.PeriodPlan)
                    .ThenInclude(periodPlan => periodPlan!.Currency);
        }
    }
}
