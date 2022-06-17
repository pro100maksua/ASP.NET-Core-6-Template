using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using $ext_safeprojectname$.Application.InterfacesRepository;

namespace $ext_safeprojectname$.Infrastructure.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string[] includeProperties = null)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking();

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

    public virtual ValueTask<TEntity> Get(int id)
    {
        return DbSet.FindAsync(id);
    }

    public virtual Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate, string[] includeProperties = null)
    {
        IQueryable<TEntity> query = DbSet;

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return query.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task Add(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public virtual Task AddRange(IEnumerable<TEntity> entities)
    {
        return DbSet.AddRangeAsync(entities);
    }

    public virtual async Task Remove(int id)
    {
        var entity = await Get(id);
        Remove(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            DbSet.Attach(entity);
        }

        DbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
        }

        DbSet.RemoveRange(entities);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }
}