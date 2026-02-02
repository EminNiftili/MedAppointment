
using System;

namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Localization
{
    internal class ResourceRespository : EfGenericRepository<ResourceEntity>, IResourceRepository
    {
        public ResourceRespository(MedicalAppointmentContext medicalAppointmentContext) : base(medicalAppointmentContext, medicalAppointmentContext.Set<ResourceEntity>(), true)
        {
        }

        public async Task<ResourceEntity?> GetByKeyAsync(string key)
        {
            var query = IncludeQuery(Query, true);
            return await query.FirstOrDefaultAsync(x => x.Key == key);
        }

        protected override IQueryable<ResourceEntity> IncludeQuery(IQueryable<ResourceEntity> query)
        {
            return query.Include(x => x.Translations)
                            .ThenInclude(x => x.Language);
        }
    }
}
