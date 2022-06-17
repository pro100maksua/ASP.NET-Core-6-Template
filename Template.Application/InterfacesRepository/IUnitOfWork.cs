namespace $ext_safeprojectname$.Application.InterfacesRepository;

public interface IUnitOfWork
{
    Task SaveChanges();   
}