using class_library_disc.Models.Sql;
using Migrator.Data;


namespace Migrator.Services
{
    public class MigrateToNeo4J
    {
        private readonly Neo4JConnection _neo4j;
        private readonly TimeSpan utcPlusOne = TimeSpan.FromHours(1);

        public MigrateToNeo4J(Neo4JConnection neo4j)
        {
            _neo4j = neo4j;
        }

        public async Task MigrateCompanysAsync(FetchedData data)
        {
            string nodeName = "Company";
            Console.WriteLine($"Migrating {data.Companies.Count} {nodeName} to Neo4j...");
            foreach (var d in data.Companies)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", d.Id },
                    { "name", d.Name },
                    { "location", d.Location },
                    { "buisiness_field", d.BusinessField }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);
            }
            Console.WriteLine($"{nodeName} migration completed");
        }

        public async Task MigrateDepartmentsAsync(FetchedData data)
        {
            string nodeName = "Department";
            Console.WriteLine($"Migrating {data.Departments.Count} {nodeName} to Neo4j...");
            foreach (var d in data.Departments)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", d.Id },
                    { "name", d.Name },
                    { "description", d.Description }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);
            }
            Console.WriteLine($"{nodeName} migration completed");
        }

        public async Task MigrateDiscProfilesAsync(FetchedData data)
        {
            string nodeName = "DiscProfile";
            Console.WriteLine($"Migrating {data.DiscProfiles.Count} {nodeName} to Neo4j...");
            foreach (var d in data.DiscProfiles)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", d.Id },
                    { "name", d.Name },
                    { "description", d.Description },
                    { "color", d.Color }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);
            }
            Console.WriteLine($"{nodeName} migration completed");
        }
        public async Task MigrateEmployeePrivateDataAsync(FetchedData data)
        {
            string nodeName = "EmployeePrivateData";
            Console.WriteLine($"Migrating {data.EmployeePrivateData.Count} {nodeName} to Neo4j...");
            foreach (var d in data.EmployeePrivateData)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", d.EmployeeId },
                    { "private_email", d.PrivateEmail },
                    { "private_phone", d.PrivatePhone },
                    { "cpr", d.Cpr }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);
            }
            Console.WriteLine($"{nodeName} migration completed");
        }
        public async Task MigrateCompletionIntervalsAsync(FetchedData data)
        {
            string nodeName = "CompletionInterval";
            Console.WriteLine($"Migrating {data.CompletionIntervals.Count} {nodeName} to Neo4j...");
            foreach (var d in data.CompletionIntervals)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "time_to_complete", d.TimeToComplete },
                    { "id", d.Id }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);
            }
            Console.WriteLine($"{nodeName} migration completed");
        }

        public async Task MigrateStressMeasuresAsync(FetchedData data)
        {
            string nodeName = "StressMeassure";
            Console.WriteLine($"Migrating {data.StressMeasures.Count} {nodeName} to Neo4j...");
            foreach (var d in data.StressMeasures)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", d.Id },
                    { "description", d.Description },
                    { "measure", d.Measure }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);

            }

            Console.WriteLine($"{nodeName} migration completed");
        }
        public async Task MigratePositionsAsync(FetchedData data)
        {
            string nodeName = "Position";
            Console.WriteLine($"Migrating {data.Positions.Count} {nodeName} to Neo4j...");
            foreach (var d in data.Positions)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", d.Id },
                    { "name", d.Name },
                    { "description", d.Description }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);
            }
            Console.WriteLine($"{nodeName} migration completed");
        }

        public async Task MigrateUsersAsync(FetchedData data)
        {
            string nodeName = "User";
            Console.WriteLine($"Migrating {data.Users.Count} {nodeName} to Neo4j...");
            foreach (var d in data.Users)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", d.EmployeeId },
                    { "username", d.Username },
                    { "password_hash", d.PasswordHash },
                    { "requires_reset", d.RequiresReset }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);

            }
            Console.WriteLine($"{nodeName} migration completed");
        }

        public async Task MigrateUserRolesAsync(FetchedData data)
        {
            string nodeName = "UserRole";
            Console.WriteLine($"Migrating {data.UserRoles.Count} {nodeName} to Neo4j...");
            foreach (var d in data.UserRoles)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", d.Id },
                    { "name", d.Name },
                    { "description", d.Description }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);
                foreach (var user in d.Users)
                {
                    await _neo4j.CreateRelationshipAsync("User", user.EmployeeId, "HAS_PERMISSION_AS", "UserRole", d.Id);

                }


            }
            Console.WriteLine($"{nodeName} migration completed");
        }
        public async Task MigrateProjectTasksAsync(FetchedData data)
        {
            string nodeName = "ProjectTask";
            Console.WriteLine($"Migrating {data.ProjectTasks.Count} {nodeName} to Neo4j...");
            foreach (var pt in data.ProjectTasks)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", pt.Id },
                    { "name", pt.Name },
                    { "completed", pt.Completed },
                    { "time_of_completion", pt.TimeOfCompletion.HasValue
                        ? new DateTimeOffset(pt.TimeOfCompletion.Value, utcPlusOne)
                        : null}
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);
                foreach (var sm in pt.StressMeasures)
                {
                    await _neo4j.CreateRelationshipAsync("ProjectTask", pt.Id, "MEASSURED_TO", "StressMeassure", sm.Id);
                }
                if (pt.TimeToComplete != null)
                {
                    await _neo4j.CreateRelationshipAsync("ProjectTask", pt.Id, "FINNISHED_IN", "CompletionInterval", pt.TimeToComplete.Id);
                }

            }
            Console.WriteLine($"{nodeName} migration completed");
        }
        public async Task MigrateProjectsAsync(FetchedData data)
        {
            string nodeName = "Project";
            Console.WriteLine($"Migrating {data.Projects.Count} {nodeName} to Neo4j...");
            foreach (var project in data.Projects)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", project.Id },
                    { "name", project.Name },
                    { "description", project.Description },
                    { "deadline", project.Deadline.HasValue
                        ? new DateTimeOffset(project.Deadline.Value, utcPlusOne)
                        : null },
                    { "completed", project.Completed },
                    { "employees_needed", project.EmployeesNeeded }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);
                foreach (var pt in project.ProjectTasks)
                {
                    await _neo4j.CreateRelationshipAsync("Project", project.Id, "CONSISTS_OF", "ProjectTask", pt.Id);
                }
                foreach (var dp in project.ProjectsDiscProfiles)
                {
                    await _neo4j.CreateRelationshipAsync("Project", project.Id, "NEEDS", "DiscProfile", dp.DiscProfileId);
                }
            }
            Console.WriteLine($"{nodeName} migration completed");
        }
        public async Task MigrateEmployeesAsync(FetchedData data)
        {
            string nodeName = "Employee";
            Console.WriteLine($"Migrating {data.Employees.Count} {nodeName} to Neo4j...");
            foreach (var employee in data.Employees)
            {
                var dict = new Dictionary<string, object?>
                {
                    { "id", employee.Id },
                    { "work_email", employee.WorkEmail },
                    { "work_phone", employee.WorkPhone },
                    { "first_name", employee.FirstName },
                    { "last_name", employee.LastName },
                    { "image_path", employee.ImagePath }
                };
                await _neo4j.CreateNodeAsync(nodeName, dict);
                foreach (var pte in employee.ProjectTasksEmployees)
                {
                    if (!pte.Task.Completed)
                    {
                        await _neo4j.CreateRelationshipAsync("Employee", employee.Id, "IN_PROGRESS", "ProjectTask", pte.TaskId);
                    }
                    else
                    {
                        await _neo4j.CreateRelationshipAsync("Employee", employee.Id, "HAS_COMPLETED", "ProjectTask", pte.TaskId);
                    }
                }
                foreach (var sm in employee.StressMeasures)
                {
                    await _neo4j.CreateRelationshipAsync("StressMeassure", sm.Id, "MEASSURED_BY", "Employee", employee.Id);
                }
                if (employee.DiscProfileId != null)
                {
                    await _neo4j.CreateRelationshipAsync("Employee", employee.Id, "BELONGS_TO", "DiscProfile", employee.DiscProfileId);
                }

                if (employee.Department != null)
                {
                    await _neo4j.CreateRelationshipAsync("Employee", employee.Id, "WORKS_IN", "Department", employee.DepartmentId);
                }
                else
                {
                    Console.WriteLine("Error between employee and Department");
                }
                if (employee.PositionId != null)
                {
                    await _neo4j.CreateRelationshipAsync("Employee", employee.Id, "OCCUPIES", "Position", employee.PositionId);
                }

                if (employee.EmployeePrivateDatum != null)
                {
                    await _neo4j.CreateRelationshipAsync("Employee", employee.Id, "HAS", "EmployeePrivateData", employee.EmployeePrivateDatum.EmployeeId);
                }
                else
                {
                    Console.WriteLine("Error between employee and EmployeePrivateData");
                }


                foreach (var project in employee.EmployeesProjects)
                {
                    if (project.IsProjectManager) await _neo4j.CreateRelationshipAsync("Employee", employee.Id, "IS_MANAGER", "Project", project.ProjectId);
                    if (project.CurrentlyWorkingOn) await _neo4j.CreateRelationshipAsync("Employee", employee.Id, "CURRENTLY_WORKING_ON", "Project", project.ProjectId);
                    else await _neo4j.CreateRelationshipAsync("Employee", employee.Id, "HAS_WORKED_ON", "Project", project.ProjectId);
                }
                if (employee.User != null)
                {
                    await _neo4j.CreateRelationshipAsync("Employee", employee.Id, "IS_A", "User", employee.User.EmployeeId);

                }
                else
                {
                    Console.WriteLine("Error between employee and user");
                }
            }
            Console.WriteLine($"{nodeName} migration completed");
        }



        public async Task MigrateDataToNeo4jAsync(FetchedData data)
        {
            Console.WriteLine("MigrateDataToNeo4jAsync function");

            if (data == null)
            {
                Console.WriteLine("No data fetched to migrate.");
                return;
            }
            await MigrateCompanysAsync(data);
            await MigrateCompletionIntervalsAsync(data);
            await MigrateDepartmentsAsync(data);
            await MigrateDiscProfilesAsync(data);
            await MigrateEmployeePrivateDataAsync(data);
            await MigratePositionsAsync(data);
            await MigrateUsersAsync(data);
            await MigrateUserRolesAsync(data);
            await MigrateStressMeasuresAsync(data);
            await MigrateProjectTasksAsync(data);
            await MigrateProjectsAsync(data);
            await MigrateEmployeesAsync(data);

        }
    }
}
