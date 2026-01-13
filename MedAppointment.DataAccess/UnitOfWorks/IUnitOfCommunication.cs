namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfCommunication : IUnitOfWork
    {
        IChatHistoryRepository ChatHistory { get; }
        IChatRepository Chat { get; }
        IMeetRepository Meet { get; }
    }
}
