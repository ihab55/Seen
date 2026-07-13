namespace SeenCL.Interfaces
{
    /// <summary>
    /// Generic repository contract for basic CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type managed by this repository.</typeparam>
    /// <typeparam name="TKey">The primary-key type (usually int).</typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        TEntity? GetById(TKey id);
        TKey Create(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TKey id);
    }
}
