namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfDoctor : IUnitOfWork
    {
        IDoctorRepository Doctor { get; }
        IDaySchemaRepository DaySchema { get; }
        IWeeklySchemaRepository WeeklySchema { get; }
        IDayBreakRepository DayBreak { get; }
    }
}
