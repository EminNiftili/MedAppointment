namespace MedAppointment.DataAccess.Repositories.Client
{
    public interface IPersonRepository : IGenericRepository<PersonEntity>
    {
        Task<PersonEntity?> FindByUsernameAsync(string name, bool calIncludeAll = false);
    }
}
