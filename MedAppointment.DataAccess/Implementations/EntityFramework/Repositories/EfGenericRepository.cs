namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories
{
    internal abstract class EfGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected readonly MedicalAppointmentContext MedicalAppointmentContext;
        protected readonly DbSet<TEntity> Entities;
        protected readonly IQueryable<TEntity> Query;
        protected readonly bool CanIncludeAll;

        internal EfGenericRepository(MedicalAppointmentContext medicalAppointmentContext, DbSet<TEntity> entities, bool canIncludeAll)
        {
            MedicalAppointmentContext = medicalAppointmentContext;
            Entities = entities;
            CanIncludeAll = canIncludeAll;
            Query = entities.AsQueryable().Where(x => !x.IsDeleted);
        }



        public void Add(TEntity entity)
        {
            Entities.Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return Query.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query.AnyAsync(predicate);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool canIncludeAll = true)
        {
            var query = IncludeQuery(Query, canIncludeAll);
            return query.Where(predicate).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool canIncludeAll = true)
        {
            var query = IncludeQuery(Query, canIncludeAll);
            return await query.Where(predicate).ToListAsync();
        }

        public TEntity? FindFirst(Expression<Func<TEntity, bool>> predicate, bool canIncludeAll = true)
        {
            var query = IncludeQuery(Query, canIncludeAll);
            return query.FirstOrDefault(predicate);
        }

        public async Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> predicate, bool canIncludeAll = true)
        {
            var query = IncludeQuery(Query, canIncludeAll);
            return await query.FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<TEntity> GetAll(bool canIncludeAll = true)
        {
            var query = IncludeQuery(Query, canIncludeAll);
            return query.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool canIncludeAll = true)
        {
            var query = IncludeQuery(Query, canIncludeAll);
            return await query.ToListAsync();
        }

        public TEntity? GetById(long id, bool canIncludeAll = true)
        {
            return FindFirst(x => x.Id == id, canIncludeAll);
        }

        public async Task<TEntity?> GetByIdAsync(long id, bool canIncludeAll = true)
        {
            return await FindFirstAsync(x => x.Id == id, canIncludeAll);
        }

        public void Remove(long id)
        {
            var entity = GetById(id, false);
            if(entity == null)
            {
                throw new ArgumentNullException($"Entity Not found in DataBase");
            }
            entity.IsDeleted = true;
            Update(entity);
        }

        public async Task RemoveAsync(long id)
        {
            var entity = await GetByIdAsync(id, false);
            if (entity == null)
            {
                throw new ArgumentNullException($"Entity Not found in DataBase");
            }
            entity.IsDeleted = true;
            Update(entity);
        }

        public void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        protected IQueryable<TEntity> IncludeQuery(IQueryable<TEntity> query, bool canIncludeAll)
        {
            if (CanIncludeAll && canIncludeAll)
            {
                return IncludeQuery(query);
            }
            else
            {
                return query;
            }
        }

        protected abstract IQueryable<TEntity> IncludeQuery(IQueryable<TEntity> query);

    }
}
