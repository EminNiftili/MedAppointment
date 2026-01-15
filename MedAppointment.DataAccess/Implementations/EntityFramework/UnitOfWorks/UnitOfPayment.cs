namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfPayment : EfUnitOfWork, IUnitOfPayment
    {
        public UnitOfPayment(MedicalAppointmentContext medicalAppointmentContext,
            IPaymentRepository payment) : base(medicalAppointmentContext)
        {
            Payment = payment;
        }

        public IPaymentRepository Payment { get; private set; }
    }
}
