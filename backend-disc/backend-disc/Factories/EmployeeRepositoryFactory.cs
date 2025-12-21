using backend_disc.Repositories;
using backend_disc.Repositories.Mongo;
using backend_disc.Repositories.Neo4J;

namespace backend_disc.Factories
{
    public class EmployeeRepositoryFactory : IEmployeeRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public EmployeeRepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEmployeesRepository GetRepository(string dbType) =>
            dbType.ToLower() switch
            {
                "mssql" => _serviceProvider.GetRequiredService<EmployeesRepository>(),
                "mongodb" => _serviceProvider.GetRequiredService<EmployeesMongoRepository>(),
                "neo4j" => _serviceProvider.GetRequiredService<EmployeesNeo4JRepository>(),
                _ => _serviceProvider.GetRequiredService<EmployeesRepository>()
            };
    }
}
