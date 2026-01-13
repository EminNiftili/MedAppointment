namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfService : IUnitOfWork
    {
        IAppointmentRepository Appointment { get; }
        IDayPlanRepository DayPlan { get; }
        IPeriodPlanRepository PeriodPlan { get; }
    }
}
