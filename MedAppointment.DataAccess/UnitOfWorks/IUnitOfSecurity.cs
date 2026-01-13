namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfSecurity : IUnitOfWork
    {
        IDeviceRepository Device { get; }
        ISessionRepository Session { get; }
        ITokenRepository Token { get; }
        ITraditionalUserRepository TraditionalUser { get; }
    }
}
