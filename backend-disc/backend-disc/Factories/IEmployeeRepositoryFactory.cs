using backend_disc.Repositories;

namespace backend_disc.Factories
{
    public interface IEmployeeRepositoryFactory
    {
        IEmployeesRepository GetRepository(string dbType);
    }
}