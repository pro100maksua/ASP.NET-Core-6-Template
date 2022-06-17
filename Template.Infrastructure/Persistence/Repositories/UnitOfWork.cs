using $ext_safeprojectname$.Application.InterfacesRepository;

namespace $ext_safeprojectname$.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChanges()
    {
        return _dbContext.SaveChangesAsync();
    }
}