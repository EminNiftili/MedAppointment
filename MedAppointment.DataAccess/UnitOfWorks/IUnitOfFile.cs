namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfFile : IUnitOfWork
    {
        IImageRepository Image { get; }
    }
}
