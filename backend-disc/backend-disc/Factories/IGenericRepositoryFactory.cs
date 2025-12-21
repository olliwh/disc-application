using backend_disc.Repositories;

namespace backend_disc.Factories
{
    public interface IGenericRepositoryFactory
    {
        IGenericRepository<T> GetRepository<T>(string dbType) where T : class;
    }
}