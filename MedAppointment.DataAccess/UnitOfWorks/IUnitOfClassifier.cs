namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfClassifier : IUnitOfWork
    {
        ICurrencyRepository Currency { get; }
        IPaymentTypeRepository PaymentType { get; }
        IPeriodRepository Period { get; }
        ISpecialtyRepository Specialty { get; }
    }
}
