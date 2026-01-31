namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfClient : IUnitOfWork
    {
        IAdminRepository Admin { get; }
        IPersonRepository Person { get; }
        IUserRepository User { get; }
        IOrganizationRepository Organization { get; }
        IOrganizationUserRepository OrganizationUser { get; }
    }
}
