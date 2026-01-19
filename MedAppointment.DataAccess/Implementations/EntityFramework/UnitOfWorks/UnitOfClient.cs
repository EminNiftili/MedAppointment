

namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfClient : EfUnitOfWork, IUnitOfClient
    {
        public UnitOfClient(MedicalAppointmentContext medicalAppointmentContext,
            IAdminRepository admin, 
            IDoctorRepository doctor, 
            IPersonRepository person, 
            IUserRepository user,
            IOrganizationRepository organization,
            IOrganizationUserRepository organizationUser) : base(medicalAppointmentContext)
        {
            Admin = admin;
            Doctor = doctor;
            Person = person;
            User = user;
            Organization = organization;
            OrganizationUser = organizationUser;
        }

        public IAdminRepository Admin { get; private set; }

        public IDoctorRepository Doctor { get; private set; }

        public IPersonRepository Person { get; private set; }

        public IUserRepository User { get; private set; }

        public IOrganizationRepository Organization { get; private set; }

        public IOrganizationUserRepository OrganizationUser { get; private set; }
    }
}
