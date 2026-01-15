namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Contexts.MedicalAppointment
{
    internal partial class MedicalAppointmentContext : DbContext
    {
        public MedicalAppointmentContext(DbContextOptions<MedicalAppointmentContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly((typeof(MedicalAppointmentContext)).Assembly);
        }
    }
}
