namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Contexts.MedicalAppointment
{
    internal partial class MedicalAppointmentContext : DbContext
    {
        internal MedicalAppointmentContext(DbContextOptions<MedicalAppointmentContext> options)
            : base(options)
        {
        }
    }
}
