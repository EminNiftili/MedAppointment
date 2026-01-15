namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Contexts.MedicalAppointment
{
    internal partial class MedicalAppointmentContext : DbContext
    {
        public MedicalAppointmentContext(DbContextOptions<MedicalAppointmentContext> options)
            : base(options)
        {
        }
    }
}
