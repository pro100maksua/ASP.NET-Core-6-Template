using System.Linq.Expressions;

namespace Template.Application.InterfacesRepository;

public interface ISimpleRepository
{
    Task<IEnumerable<TEntity>> GetAll<TEntity>(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string[] includeProperties = null)
        where TEntity : class;

    ValueTask<TEntity> Get<TEntity>(int id)
        where TEntity : class;

    Task<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate, string[] includeProperties = null)
        where TEntity : class;

    Task Add<TEntity>(TEntity entity)
        where TEntity : class;
    Task AddRange<TEntity>(IEnumerable<TEntity> entity)
        where TEntity : class;

    Task Remove<TEntity>(int id)
        where TEntity : class;
    void Remove<TEntity>(TEntity entity)
        where TEntity : class;
    void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class;
}