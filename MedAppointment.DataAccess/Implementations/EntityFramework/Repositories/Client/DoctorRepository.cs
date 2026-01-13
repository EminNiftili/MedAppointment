namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client
{
    internal class DoctorRepository : EfGenericRepository<DoctorEntity>, IDoctorRepository
    {
        internal DoctorRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<DoctorEntity>(), true)
        {
        }

        protected override IQueryable<DoctorEntity> IncludeQuery(IQueryable<DoctorEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
