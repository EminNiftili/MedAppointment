

namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Doctor
{
    internal class DaySchemaRepository : EfGenericRepository<DaySchemaEntity>, IDaySchemaRepository
    {
        public DaySchemaRepository(MedicalAppointmentContext medicalAppointmentContext) : base(medicalAppointmentContext, medicalAppointmentContext.Set<DaySchemaEntity>(), false)
        {
        }

        protected override IQueryable<DaySchemaEntity> IncludeQuery(IQueryable<DaySchemaEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
