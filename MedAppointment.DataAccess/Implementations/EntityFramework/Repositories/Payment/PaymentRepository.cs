namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Payment
{
    internal class PaymentRepository : EfGenericRepository<PaymentEntity>, IPaymentRepository
    {
        public PaymentRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<PaymentEntity>(), true)
        {
        }

        protected override IQueryable<PaymentEntity> IncludeQuery(IQueryable<PaymentEntity> query)
        {
            return query.Include(payment => payment.PaymentType);
        }
    }
}
