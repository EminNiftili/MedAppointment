namespace MedAppointment.DataAccess.Repositories.Client
{
    public interface IPersonRepository : IGenericRepository<PersonEntity>
    {
        PersonEntity? FindByUsername(string name, bool calIncludeAll = false);
    }
}
