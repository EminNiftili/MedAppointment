namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfDoctor : EfUnitOfWork, IUnitOfDoctor
    {
        public UnitOfDoctor(IDoctorRepository doctor, 
            IDaySchemaRepository daySchema, 
            IWeeklySchemaRepository weeklySchema, 
            IDayBreakRepository dayBreak,
            MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext)
        {
            Doctor = doctor;
            DaySchema = daySchema;
            WeeklySchema = weeklySchema;
            DayBreak = dayBreak;
        }

        public IDoctorRepository Doctor { get; private set; }

        public IDaySchemaRepository DaySchema { get; private set; }

        public IWeeklySchemaRepository WeeklySchema { get; private set; }

        public IDayBreakRepository DayBreak { get; private set; }
    }
}
