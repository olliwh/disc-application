using backend_disc.Repositories;
using backend_disc.Services;
using class_library_disc.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : GenericController<Employee>
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IGenericRepository<Employee> repository, IEmployeeService employeeService) : base(repository)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Helper function to create password hashes for inserting new emplyees to DB using sql
        /// returning IActionResult<string> gives error
        /// </summary>
        /// <returns>String</returns>
        [HttpGet("getDefaultPass")]
        public ActionResult<string> getDefaultPass()
        {
            string password = "Pass@word1";
            string hash = Argon2.Hash(password);
            return Ok(hash);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public override async Task<IActionResult> GetAll(
            [FromQuery] int? departmentId = null,
            [FromQuery] int? discProfileId = null,
            [FromQuery] int? positionId = null)
        {
            var employees = await _employeeService.GetAll(departmentId, discProfileId, positionId);
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(5));
            return Ok(employees);
        }
    }
}
