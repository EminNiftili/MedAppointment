namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfPayment : IUnitOfWork
    {
        IPaymentRepository Payment { get; }

    }
}
