namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfClient : IUnitOfWork
    {
        IAdminRepository Admin { get; }
        IDoctorRepository Doctor { get; }
        IPersonRepository Person { get; }
        IUserRepository User { get; }
        IOrganizationRepository Organization { get; }
        IOrganizationUserRepository OrganizationUser { get; }
    }
}
