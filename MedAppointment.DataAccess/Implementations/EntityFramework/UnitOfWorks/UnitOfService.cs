namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfService : EfUnitOfWork, IUnitOfService
    {
        public UnitOfService(MedicalAppointmentContext medicalAppointmentContext,
            IAppointmentRepository appointment, 
            IDayPlanRepository dayPlan,
            IPeriodPlanRepository periodPlan) : base(medicalAppointmentContext)
        {
            Appointment = appointment;
            DayPlan = dayPlan;
            PeriodPlan = periodPlan;
        }

        public IAppointmentRepository Appointment { get; private set; }

        public IDayPlanRepository DayPlan { get; private set; }

        public IPeriodPlanRepository PeriodPlan { get; private set; }
    }
}
