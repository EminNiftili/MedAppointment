
namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfCommunication : EfUnitOfWork, IUnitOfCommunication
    {
        public UnitOfCommunication(MedicalAppointmentContext medicalAppointmentContext,
            IChatHistoryRepository chatHistory, 
            IChatRepository chat, 
            IMeetRepository meet) : base(medicalAppointmentContext)
        {
            ChatHistory = chatHistory;
            Chat = chat;
            Meet = meet;
        }

        public IChatHistoryRepository ChatHistory { get; private set; }

        public IChatRepository Chat { get; private set; }

        public IMeetRepository Meet { get; private set; }
    }
}
