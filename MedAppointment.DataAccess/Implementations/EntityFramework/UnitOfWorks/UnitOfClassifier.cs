
namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfClassifier : EfUnitOfWork, IUnitOfClassifier
    {
        public UnitOfClassifier(MedicalAppointmentContext medicalAppointmentContext,
            ICurrencyRepository currency, 
            IPaymentTypeRepository paymentType, 
            IPeriodRepository period, 
            ISpecialtyRepository specialty) : base(medicalAppointmentContext)
        {
            Currency = currency;
            PaymentType = paymentType;
            Period = period;
            Specialty = specialty;
        }

        public ICurrencyRepository Currency { get; private set; }

        public IPaymentTypeRepository PaymentType { get; private set; }

        public IPeriodRepository Period { get; private set; }

        public ISpecialtyRepository Specialty { get; private set; }
    }
}
