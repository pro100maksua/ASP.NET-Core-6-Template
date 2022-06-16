namespace Template.Application.InterfacesRepository;

public interface IUnitOfWork
{
    Task SaveChanges();   
}