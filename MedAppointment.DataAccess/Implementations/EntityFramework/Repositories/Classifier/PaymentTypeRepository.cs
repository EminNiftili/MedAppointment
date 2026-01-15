namespace MedAppointment.DataAccess.Repositories.Classifier
{
    internal class PaymentTypeRepository : EfGenericRepository<PaymentTypeEntity>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<PaymentTypeEntity>(), false)
        {
        }

        protected override IQueryable<PaymentTypeEntity> IncludeQuery(IQueryable<PaymentTypeEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
