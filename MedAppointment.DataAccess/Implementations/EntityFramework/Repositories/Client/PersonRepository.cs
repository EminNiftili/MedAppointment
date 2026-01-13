namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client
{
    internal class PersonRepository : EfGenericRepository<PersonEntity>, IPersonRepository
    {
        internal PersonRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<PersonEntity>(), true)
        {
        }

        protected override IQueryable<PersonEntity> IncludeQuery(IQueryable<PersonEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
