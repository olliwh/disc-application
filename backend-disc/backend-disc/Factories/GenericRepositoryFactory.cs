using backend_disc.Repositories;
using backend_disc.Repositories.Mongo;
using backend_disc.Repositories.Neo4J;

namespace backend_disc.Factories
{
    public class GenericRepositoryFactory : IGenericRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public GenericRepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IGenericRepository<T> GetRepository<T>(string dbType) where T : class
        {
            return dbType.ToLower() switch
            {
                "mssql" => _serviceProvider.GetRequiredService<GenericRepository<T>>(),
                "mongodb" => _serviceProvider.GetRequiredService<GenericMongoRepository<T>>(),
                "neo4j" => GetNeo4JRepository<T>(),
                _ => _serviceProvider.GetRequiredService<GenericRepository<T>>()
            };
        }
        private IGenericRepository<T> GetNeo4JRepository<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<IGenericRepository<T>>();
        }
    }
}
