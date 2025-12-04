using backend_disc.Repositories;
using backend_disc.Repositories.Mongo;
using backend_disc.Repositories.Neo4J;

namespace backend_disc.Factories
{
    public class EmployeeRepositoryFactory : IEmployeeRepositoryFactory
    {
        private readonly EmployeesRepository _sqlRepo;
        private readonly EmployeesMongoRepository _mongoRepo;
        private readonly EmployeesNeo4JRepository _neoRepo;

        public EmployeeRepositoryFactory(
            EmployeesRepository sqlRepo,
            EmployeesMongoRepository mongoRepo,
            EmployeesNeo4JRepository neoRepo)
        {
            _sqlRepo = sqlRepo;
            _mongoRepo = mongoRepo;
            _neoRepo = neoRepo;
        }

        public IEmployeesRepository GetRepository(string dbType) =>
            dbType.ToLower() switch
            {
                "mssql" => _sqlRepo,
                "mongodb" => _mongoRepo,
                "neo4j" => _neoRepo,
                _ => _sqlRepo    // default
            };
    }
}
