namespace MedAppointment.DataAccess.Repositories.Classifier
{
    internal class SpecialtyRepository : EfGenericRepository<SpecialtyEntity>, ISpecialtyRepository
    {
        public SpecialtyRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<SpecialtyEntity>(), false)
        {
        }

        protected override IQueryable<SpecialtyEntity> IncludeQuery(IQueryable<SpecialtyEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
