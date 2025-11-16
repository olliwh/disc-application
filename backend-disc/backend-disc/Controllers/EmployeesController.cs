using backend_disc.Dtos.Employees;
using backend_disc.Repositories;
using backend_disc.Services;
using class_library_disc.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;
        public EmployeesController( IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? departmentId = null,
            [FromQuery] int? discProfileId = null,
            [FromQuery] int? positionId = null,
            [FromQuery] string? search = null,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var employees = await _employeeService.GetAll(departmentId, discProfileId, positionId, search, pageIndex, pageSize);
            //await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2));
            return Ok(employees);
        }
        /// <summary>
        /// Creates a new employee only admin users are allowed to create employees
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//not admin role
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//in valid token
        //NullReferenceException if fk doesnt exist
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateNewEmployee dto)
        {
            try
            {
                var employee = await _employeeService.CreateEmployee(dto);
                if (employee == null)
                {
                    _logger.LogError("Employee service returned null");
                    return StatusCode(500, new { message = "Failed to create employee" });
                }

                return CreatedAtAction(nameof(GetAll), new { id = employee.Id }, employee);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input for creating employee");
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found when creating employee");
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Database operation failed when creating employee");
                return StatusCode(500, new { message = "Failed to create employee due to database error" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating employee");
                return StatusCode(500, new { message = "An unexpected error occurred while creating the employee" });
            }
        }
    }
}
