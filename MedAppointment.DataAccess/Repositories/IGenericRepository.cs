namespace MedAppointment.DataAccess.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        #region Create
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        #endregion

        #region Read
        TEntity? GetById(long id, bool canIncludeAll = true);
        Task<TEntity?> GetByIdAsync(long id, bool canIncludeAll = true);
        IEnumerable<TEntity> GetAll(bool canIncludeAll = true);
        Task<IEnumerable<TEntity>> GetAllAsync(bool canIncludeAll = true);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool canIncludeAll = true);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool canIncludeAll = true);
        TEntity? FindFirst(Expression<Func<TEntity, bool>> predicate, bool canIncludeAll = true);
        Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> predicate, bool canIncludeAll = true);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region Update
        void Update(TEntity entity);
        #endregion

        #region Delete
        void Remove(long id);
        Task RemoveAsync(long id);
        #endregion

    }
}
