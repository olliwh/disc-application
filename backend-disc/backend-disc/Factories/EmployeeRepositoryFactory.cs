using backend_disc.Repositories;

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
                "mongodb" => _serviceProvider.GetRequiredKeyedService<IEmployeesRepository>("mongodb"),
                "neo4j" => _serviceProvider.GetRequiredKeyedService<IEmployeesRepository>("neo4j"),
                _ => _serviceProvider.GetRequiredService<IEmployeesRepository>()
            };
    }
}
