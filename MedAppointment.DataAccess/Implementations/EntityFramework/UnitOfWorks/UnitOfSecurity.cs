namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfSecurity : EfUnitOfWork, IUnitOfSecurity
    {
        public UnitOfSecurity(MedicalAppointmentContext medicalAppointmentContext,
            IDeviceRepository device, 
            ISessionRepository session, 
            ITokenRepository token, 
            ITraditionalUserRepository traditionalUser) : base(medicalAppointmentContext)
        {
            Device = device;
            Session = session;
            Token = token;
            TraditionalUser = traditionalUser;
        }

        public IDeviceRepository Device { get; private set; }

        public ISessionRepository Session { get; private set; }

        public ITokenRepository Token { get; private set; }

        public ITraditionalUserRepository TraditionalUser { get; private set; }
    }
}
