using backend_disc.Repositories;
using class_library_disc.Models.Sql;

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
            string db = dbType?.ToLower() ?? "mssql";

            if (typeof(T) == typeof(User))
            {
                return db switch
                {
                    "mongodb" => (IGenericRepository<T>)_serviceProvider.GetRequiredKeyedService<IUserRepository>("mongodb"),
                    "neo4j" => (IGenericRepository<T>)_serviceProvider.GetRequiredKeyedService<IUserRepository>("neo4j"),
                    _ => (IGenericRepository<T>)_serviceProvider.GetRequiredService<IUserRepository>()
                };
            }

            return db switch
            {
                "mongodb" => _serviceProvider.GetRequiredKeyedService<IGenericRepository<T>>("mongodb"),
                "neo4j" => _serviceProvider.GetRequiredKeyedService<IGenericRepository<T>>("neo4j"),
                _ => _serviceProvider.GetRequiredService<GenericRepository<T>>() 
            };
        }
    }
}
