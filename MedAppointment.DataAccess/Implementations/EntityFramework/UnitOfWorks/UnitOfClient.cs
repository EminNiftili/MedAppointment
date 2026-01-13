
namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfClient : EfUnitOfWork, IUnitOfClient
    {
        internal UnitOfClient(MedicalAppointmentContext medicalAppointmentContext,
            IAdminRepository admin, 
            IDoctorRepository doctor, 
            IPersonRepository person, 
            IUserRepository user) : base(medicalAppointmentContext)
        {
            Admin = admin;
            Doctor = doctor;
            Person = person;
            User = user;
        }

        public IAdminRepository Admin { get; private set; }

        public IDoctorRepository Doctor { get; private set; }

        public IPersonRepository Person { get; private set; }

        public IUserRepository User { get; private set; }
    }
}
