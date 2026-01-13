namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
