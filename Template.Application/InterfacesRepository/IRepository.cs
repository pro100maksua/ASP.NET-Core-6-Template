using System.Linq.Expressions;

namespace $ext_safeprojectname$.Application.InterfacesRepository;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string[] includeProperties = null);

    ValueTask<TEntity> Get(int id);
    Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate, string[] includeProperties = null);

    Task Add(TEntity entity);
    Task AddRange(IEnumerable<TEntity> entity);

    Task Remove(int id);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
}