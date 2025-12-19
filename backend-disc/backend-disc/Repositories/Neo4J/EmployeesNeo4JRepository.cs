using backend_disc.Models;
using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Data;
using class_library_disc.Models.Sql;
using Neo4j.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend_disc.Repositories.Neo4J
{
    public class EmployeesNeo4JRepository : IEmployeesRepository
    {
        private readonly IDriver _driver;
        private readonly ILogger<EmployeesNeo4JRepository> _logger;
        private readonly string dbName = "neo4j";

        public EmployeesNeo4JRepository(IDriver driver, ILogger<EmployeesNeo4JRepository> logger)
        {
            _driver = driver;
            _logger = logger;
        }
        public async Task<PaginatedList<Employee>> GetAll(
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
                var whereClause = "";
                // need to be reight after match employee
                if (!string.IsNullOrWhiteSpace(search))
                {
                    whereClause = "WHERE (toLower(e.first_name) CONTAINS toLower($search) OR toLower(e.last_name) CONTAINS toLower($search))";
                    parameters["search"] = search;
                }
                matchClauses.Add(whereClause);

                if (departmentId.HasValue)
                {
                    matchClauses.Add("MATCH (e)-[:WORKS_IN]->(d:Department {id: $departmentId})");
                    parameters["departmentId"] = departmentId.Value;
                }
                else
                {
                    matchClauses.Add("OPTIONAL MATCH (e)-[:WORKS_IN]->(d:Department)");
                }

                if (discProfileId.HasValue)
                {
                    matchClauses.Add("MATCH (e)-[:BELONGS_TO]->(dp:DiscProfile {id: $discProfileId})");
                    parameters["discProfileId"] = discProfileId.Value;
                }
                else
                {
                    matchClauses.Add("OPTIONAL MATCH (e)-[:BELONGS_TO]->(dp:DiscProfile)");
                }

                if (positionId.HasValue)
                {
                    matchClauses.Add("MATCH (e)-[:OCCUPIES]->(p:Position {id: $positionId})");
                    parameters["positionId"] = positionId.Value;
                }
                else
                {
                    matchClauses.Add("OPTIONAL MATCH (e)-[:OCCUPIES]->(p:Position)");
                }

                var matchClause = string.Join("\n", matchClauses);
                string countReturn = "RETURN count(e) AS totalCount";
                string dataReturn = "RETURN e, d, dp, p\nORDER BY e.id ASC\nSKIP $skip LIMIT $limit";
                Console.WriteLine(parameters["skip"]);
                Console.WriteLine(pageIndex);

                var countResult = await session.RunAsync($"{matchClause}\n{countReturn}", parameters);
                var countRecord = await countResult.SingleAsync();
                var totalCount = countRecord["totalCount"].As<int>();

                var dataResult = await session.RunAsync($"{matchClause}\n{dataReturn}", parameters);
                var employees = new List<Employee>();

                await foreach (var record in dataResult)
                {
                    var eNode = record["e"].As<INode>();

                    INode? dNode = null;
                    if (record["d"] != null && record["d"].As<object>() != null)
                    {
                        dNode = record["d"].As<INode>();
                    }

                    INode? dpNode = null;
                    if (record["dp"] != null && record["dp"].As<object>() != null)
                    {
                        dpNode = record["dp"].As<INode>();
                    }

                    INode? pNode = null;
                    if (record["p"] != null && record["p"].As<object>() != null)
                    {
                        pNode = record["p"].As<INode>();
                    }

                    var employee = new Employee
                    {
                        Id = eNode["id"].As<int>(),
                        FirstName = eNode["first_name"].As<string>(),
                        LastName = eNode["last_name"].As<string>(),
                        WorkEmail = eNode["work_email"].As<string>(),
                        WorkPhone = eNode["work_phone"].As<string>(),
                        ImagePath = eNode["image_path"].As<string>(),
                        DepartmentId = dNode["id"].As<int>(),
                        PositionId = pNode?["id"].As<int?>(),
                        DiscProfileId = dpNode?["id"].As<int?>(),
                        DiscProfile = dpNode == null ? null : new DiscProfile
                        {
                            Id = dpNode["id"].As<int>(),
                            Name = dpNode["name"].As<string>(),
                            Color = dpNode["color"].As<string>()
                        }
                    };
                    employees.Add(employee);
                }

                return new PaginatedList<Employee>(employees, pageIndex, totalCount, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying Neo4j");
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
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
            var random = new Random();
            var uniqueId = random.Next(1, int.MaxValue);

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
                var result = await tx.RunAsync(query, new
                {
                    id = uniqueId,
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
                    DepartmentName = record["DepartmentName"].As<string?>(),
                    PrivateEmail = record["PrivateEmail"].As<string?>(),
                    PrivatePhone = record["PrivatePhone"].As<string?>(),
                    Username = record["username"].As<string?>()
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
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
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
                Console.WriteLine(ex + "Error deleting employee from Neo4j");
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
