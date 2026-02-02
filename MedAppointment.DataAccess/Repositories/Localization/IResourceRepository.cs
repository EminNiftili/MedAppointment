namespace MedAppointment.DataAccess.Repositories.Localization
{
    public interface IResourceRepository : IGenericRepository<ResourceEntity>
    {
        Task<ResourceEntity?> GetByKeyAsync(string key);
    }
}
