using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Models.Sql;
using Neo4j.Driver;

namespace backend_disc.Repositories.Neo4J
{
    public class EmployeesNeo4JRepository : IEmployeesRepository
    {
        private const string Message = "Error querying Neo4j database: {dbName}";
        private readonly IDriver _driver;
        private readonly ILogger<EmployeesNeo4JRepository> _logger;
        private readonly string dbName = Environment.GetEnvironmentVariable("NEO4J_DBNAME") ?? "neo4j";

        public EmployeesNeo4JRepository(IDriver driver, ILogger<EmployeesNeo4JRepository> logger)
        {
            _driver = driver;
            _logger = logger;
        }
        public async Task<(List<Employee>, int totalCount)> GetAll(
     int? departmentId, int? discProfileId, int? positionId,
     string? search, int pageIndex, int pageSize)
        {
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    ["skip"] = (pageIndex - 1) * pageSize,
                    ["limit"] = pageSize
                };

                var matchClauses = new List<string> { "MATCH (e:Employee)" };

                if (!string.IsNullOrWhiteSpace(search))
                {
                    matchClauses.Add("WHERE (toLower(e.first_name) CONTAINS toLower($search) OR toLower(e.last_name) CONTAINS toLower($search))");
                    parameters["search"] = search;
                }

                matchClauses.Add(departmentId.HasValue
                    ? "MATCH (e)-[:WORKS_IN]->(d:Department {id: $departmentId})"
                    : "OPTIONAL MATCH (e)-[:WORKS_IN]->(d:Department)");
                if (departmentId.HasValue) parameters["departmentId"] = departmentId.Value;

                matchClauses.Add(discProfileId.HasValue
                    ? "MATCH (e)-[:BELONGS_TO]->(dp:DiscProfile {id: $discProfileId})"
                    : "OPTIONAL MATCH (e)-[:BELONGS_TO]->(dp:DiscProfile)");
                if (discProfileId.HasValue) parameters["discProfileId"] = discProfileId.Value;

                matchClauses.Add(positionId.HasValue
                    ? "MATCH (e)-[:OCCUPIES]->(p:Position {id: $positionId})"
                    : "OPTIONAL MATCH (e)-[:OCCUPIES]->(p:Position)");
                if (positionId.HasValue) parameters["positionId"] = positionId.Value;

                var baseQuery = string.Join("\n", matchClauses);
                var unifiedQuery = $@"
                    {baseQuery}
                    WITH count(e) AS totalCount, collect({{e: e, d: d, dp: dp, p: p}}) AS allResults
                    UNWIND allResults AS result
                    WITH totalCount, result.e AS e, result.d AS d, result.dp AS dp, result.p AS p
                    ORDER BY e.id ASC
                    SKIP $skip LIMIT $limit
                    RETURN e, d, dp, p, totalCount";

                return await session.ExecuteReadAsync(async tx =>
                {
                    var dataResult = await tx.RunAsync(unifiedQuery, parameters);
                    var records = await dataResult.ToListAsync();

                    var employees = new List<Employee>();
                    int totalCount = 0;
                    if (records.Any())
                    {
                        totalCount = records[0]["totalCount"].As<int>();
                        foreach (var record in records)
                        {
                            var eNode = record["e"].As<INode>();
                            var dNode = record["d"] as INode;
                            var dpNode = record["dp"] as INode;
                            var pNode = record["p"] as INode;

                            employees.Add(new Employee
                            {
                                Id = eNode["id"].As<int>(),
                                FirstName = eNode["first_name"].As<string>(),
                                LastName = eNode["last_name"].As<string>(),
                                WorkEmail = eNode["work_email"].As<string>(),
                                WorkPhone = eNode["work_phone"].As<string>(),
                                ImagePath = eNode["image_path"].As<string>(),

                                DepartmentId = dNode?["id"].As<int?>() ?? 0,
                                PositionId = pNode?["id"].As<int?>(),
                                DiscProfileId = dpNode?["id"].As<int?>(),

                                DiscProfile = dpNode == null ? null : new DiscProfile
                                {
                                    Id = dpNode["id"].As<int>(),
                                    Name = dpNode["name"].As<string>(),
                                    Color = dpNode["color"].As<string>()
                                }
                            });
                        }
                    }
                    return (employees, totalCount);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Message, dbName);
                throw;
            }
            finally { await session.CloseAsync(); }
        }


        public async Task<bool> PhoneNumExists(string phoneNumber)
        {
            await using var session = _driver.AsyncSession();
            var result = await session.RunAsync(
                "MATCH (e:Employee) WHERE e.work_phone = $phoneNumber RETURN COUNT(e) > 0 as exists",
                new { phoneNumber }
            );
            var record = await result.SingleAsync();
            return record["exists"].As<bool>();
        }

        public async Task<Employee?> AddEmployeeSPAsync(AddEmployeeSpParams p)
        {
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));


            try
            {
                var query = @"
                CREATE (e:Employee {
                  id: $id,
                  work_email: $workEmail,
                  image_path: $imagePath,
                  work_phone: $workPhone,
                  last_name: $lastName,
                  first_name: $firstName
                })
                CREATE (epd:EmployeePrivateData {
                  id: $id,
                  private_email: $privateEmail,
                  cpr: $cpr,
                  private_phone: $privatePhone
                })
                CREATE (u:User {
                  id: $id,
                  username: $username,
                  password_hash: $passwordHash,
                  requires_reset: true
                })
                WITH e, epd, u
                MATCH (dept:Department {id: $departmentId}), (userRole:UserRole {id: 1})
                OPTIONAL MATCH (pos:Position {id: $positionId})
                OPTIONAL MATCH (disc:DiscProfile {id: $discProfileId})
                CREATE (e)-[:HAS]->(epd)
                CREATE (e)-[:HAS_EMPLOYEE_ROLE]->(u)
                CREATE (e)-[:WORKS_IN]->(dept)
                CREATE (u)-[:HAS_PERMISSION_AS]->(userRole)
                FOREACH (_ IN CASE WHEN pos IS NOT NULL THEN [1] ELSE [] END |
                CREATE (e)-[:OCCUPIES]->(pos)
                )

                FOREACH (_ IN CASE WHEN disc IS NOT NULL THEN [1] ELSE [] END |
                  CREATE (e)-[:BELONGS_TO]->(disc)
                )
                RETURN e
                ";
                var tx = await session.BeginTransactionAsync();
                var idQuery = "MATCH(n: Employee) RETURN max(n.id) as maxId";
                var idCursor = await tx.RunAsync(idQuery);
                var idRecord = await idCursor.SingleAsync();
                int nextId = idRecord["maxId"].As<int?>() ?? 0;
                nextId++;
                var result = await tx.RunAsync(query, new
                {
                    id = nextId,
                    firstName = p.FirstName,
                    lastName = p.LastName,
                    workEmail = p.WorkEmail,
                    workPhone = p.WorkPhone,
                    imagePath = p.ImagePath,
                    privateEmail = p.PrivateEmail,
                    privatePhone = p.PrivatePhone,
                    cpr = p.CPR,
                    username = p.Username,
                    passwordHash = p.PasswordHash,
                    positionId = p.PositionId ?? 0,
                    departmentId = p.DepartmentId,
                    discProfileId = p.DiscProfileId ?? 0,
                    userRoleId = p.UserRoleId
                });
                var record = await result.SingleAsync();

                await tx.CommitAsync();

                var employeeNode = record["e"].As<INode>();

                var employee = new Employee
                {
                    Id = employeeNode["id"].As<int>(),
                    FirstName = employeeNode["first_name"].As<string>(),
                    LastName = employeeNode["last_name"].As<string>(),
                    WorkEmail = employeeNode["work_email"].As<string>(),
                    WorkPhone = employeeNode["work_phone"].As<string>(),
                    ImagePath = employeeNode["image_path"].As<string>(),
                    DepartmentId = p.DepartmentId,
                    DiscProfileId = p.DiscProfileId ?? null,
                    PositionId = p.PositionId ?? null
                };

                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee in Neo4j");
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<EmployeesOwnProfile?> GetById(int id)
        {
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));
            try
            {
                var query = @"
                MATCH (e:Employee {id: $id})
                MATCH (e)-[:HAS]->(epd:EmployeePrivateData)
                MATCH (e)-[:IS_A]->(u:User)
                OPTIONAL MATCH (e)-[:BELONGS_TO]->(dp:DiscProfile)
                OPTIONAL MATCH (e)-[:OCCUPIES]->(p:Position)
                OPTIONAL MATCH (e)-[:WORKS_IN]->(d:Department)
                
                RETURN 
                  e.id as Id,
                  e.work_email as WorkEmail,
                  e.work_phone as WorkPhone,
                  (e.first_name + ' ' + e.last_name) as FullName,
                  e.image_path as ImagePath,
                  dp.name as DiscProfileName,
                  dp.color as DiscProfileColor,
                  p.name as PositionName,
                  d.name as DepartmentName,
                  epd.private_email as PrivateEmail,
                  epd.private_phone as PrivatePhone,
                  u.username as username
                ";

                var result = await session.RunAsync(query, new { id });
                var records = await result.ToListAsync();
                var record = records.FirstOrDefault();

                if (record == null)
                    return null;

                var employeeProfile = new EmployeesOwnProfile
                {
                    Id = record["Id"].As<int>(),
                    WorkEmail = record["WorkEmail"].As<string>(),
                    WorkPhone = record["WorkPhone"].As<string>(),
                    FullName = record["FullName"].As<string>(),
                    ImagePath = record["ImagePath"].As<string>(),
                    DiscProfileName = record["DiscProfileName"].As<string?>(),
                    DiscProfileColor = record["DiscProfileColor"].As<string?>(),
                    PositionName = record["PositionName"].As<string?>(),
                    DepartmentName = record["DepartmentName"].As<string>(),
                    PrivateEmail = record["PrivateEmail"].As<string>(),
                    PrivatePhone = record["PrivatePhone"].As<string>(),
                    Username = record["username"].As<string>()
                };

                return employeeProfile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying employee by id from Neo4j");
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<int?> Delete(int id)
        {
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));
            try
            {
                var query = @"
                MATCH (e:Employee {id: $id}), (ep:EmployeePrivateData {id: $id}), (u:User {id: $id})
                DETACH DELETE e, ep, u
                RETURN $id as deletedId
                ";

                var result = await session.RunAsync(query, new { id });
                var records = await result.ToListAsync();
                var record = records.FirstOrDefault();

                if (record == null)
                    return null;

                return record["deletedId"].As<int>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee from Neo4j");
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<int?> UpdatePrivateData(int id, string mail, string phone)
        {
            var session = _driver.AsyncSession(o => o.WithDatabase(dbName));
            try
            {
                var query = @"
                MATCH (e:Employee {id: $id})-[:HAS]->(epd:EmployeePrivateData)
                SET epd.private_email = $mail,
                    epd.private_phone = $phone
                RETURN $id as updatedId
                ";

                var result = await session.RunAsync(query, new { id, mail, phone });
                var records = await result.ToListAsync();
                var record = records.FirstOrDefault();

                if (record == null)
                    return null;

                return record["updatedId"].As<int>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee private data in Neo4j");
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

    }
}
