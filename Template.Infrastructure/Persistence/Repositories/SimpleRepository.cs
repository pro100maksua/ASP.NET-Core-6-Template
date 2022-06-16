using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Template.Application.InterfacesRepository;

namespace Template.Infrastructure.Persistence.Repositories;

public class SimpleRepository : ISimpleRepository
{
    private readonly ApplicationDbContext _context;

    public SimpleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAll<TEntity>(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string[] includeProperties = null)
        where TEntity : class
    {
        var query = _context.Set<TEntity>().AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }

    public ValueTask<TEntity> Get<TEntity>(int id)
        where TEntity : class
    {
        return _context.Set<TEntity>().FindAsync(id);
    }

    public Task<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate, string[] includeProperties = null)
        where TEntity : class
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return query.FirstOrDefaultAsync(predicate);
    }

    public async Task Add<TEntity>(TEntity entity)
        where TEntity : class
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public Task AddRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        return _context.Set<TEntity>().AddRangeAsync(entities);
    }

    public async Task Remove<TEntity>(int id)
        where TEntity : class
    {
        var entity = await Get<TEntity>(id);
        Remove(entity);
    }

    public void Remove<TEntity>(TEntity entity)
        where TEntity : class
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _context.Set<TEntity>().Attach(entity);
        }

        _context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        foreach (var entity in entities)
        {
            Remove(entity);
        }
    }
}