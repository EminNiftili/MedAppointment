
namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal abstract class EfUnitOfWork : IUnitOfWork
    {
        protected readonly MedicalAppointmentContext MedicalAppointmentContext;

        internal EfUnitOfWork(MedicalAppointmentContext medicalAppointmentContext)
        {
            MedicalAppointmentContext = medicalAppointmentContext;
        }

        public void SaveChanges()
        {
            MedicalAppointmentContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await MedicalAppointmentContext.SaveChangesAsync();
        }
    }
}
