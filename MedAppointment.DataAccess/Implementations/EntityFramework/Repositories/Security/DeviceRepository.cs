namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Security
{
    internal class DeviceRepository : EfGenericRepository<DeviceEntity>, IDeviceRepository
    {
        internal DeviceRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<DeviceEntity>(), false)
        {
        }

        protected override IQueryable<DeviceEntity> IncludeQuery(IQueryable<DeviceEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
