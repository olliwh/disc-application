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

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetAll(
        //    [FromQuery] int? departmentId = null,
        //    [FromQuery] int? discProfileId = null,
        //    [FromQuery] int? positionId = null,
        //    [FromQuery] string? search = null,
        //    [FromQuery] int pageIndex = 1,
        //    [FromQuery] int pageSize = 12)
        //{
        //    var employees = await _employeeService.GetAll(departmentId, discProfileId, positionId, search, pageIndex, pageSize);
        //    return Ok(employees);
        //}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
        [FromQuery] string db = "mssql",
        [FromQuery] int? departmentId = null,
        [FromQuery] int? discProfileId = null,
        [FromQuery] int? positionId = null,
        [FromQuery] string? search = null,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 12)
            {
                var employees = await _employeeService.GetAll(db, departmentId, discProfileId, positionId, search, pageIndex, pageSize);
                return Ok(employees);
            }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        public virtual async Task<IActionResult> GetById(int id, [FromQuery] string db = "mssql")
        {
            // Get employeeId from token
            var employeeIdFromToken = User.FindFirst("employeeId")?.Value;

            
            if (!int.TryParse(employeeIdFromToken, out var tokenEmployeeId))
            {
                _logger.LogWarning("Invalid or missing employeeId in token");
                return Unauthorized(new { message = "Invalid token - missing employeeId claim" });
            }

            // Check if requested ID matches token ID (unless user is Admin)
            if (id != tokenEmployeeId)
            {
                _logger.LogWarning("User {TokenEmployeeId} attempted to access employee {EmployeeId}", tokenEmployeeId, id);
                return Forbid();
            }

            var view = await _employeeService.GetByIdAsync(db, id);
            if (view == null) return NotFound();
            return Ok(view);
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
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateNewEmployee dto, [FromQuery] string db = "mssql")
        {
            try
            {
                var employee = await _employeeService.CreateEmployee(db, dto);
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
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating employee");
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Delete(int id, [FromQuery] string db = "mssql")
        {
            var deleted = await _employeeService.DeleteAsync(db,id);
            if (deleted == null) return NotFound();
            return Ok(deleted);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        public virtual async Task<IActionResult> Update(int id, [FromBody] UpdatePrivateDataDto updateDto, [FromQuery] string db = "mssql")
        {
            // Get employeeId from token
            var employeeIdFromToken = User.FindFirst("employeeId")?.Value;
            if (!int.TryParse(employeeIdFromToken, out var tokenEmployeeId))
            {
                _logger.LogWarning("Invalid or missing employeeId in token");
                return Unauthorized(new { message = "Invalid token" });
            }

            if (id != tokenEmployeeId)
            {
                _logger.LogWarning("User {TokenEmployeeId} attempted to update employee {EmployeeId}", tokenEmployeeId, id);
                return Forbid();
            }

            try
            {
                var updated = await _employeeService.UpdatePrivateDataAsync(db, id, updateDto);
                return Ok(new { id = updated });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input for updating employee");
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Employee not found");
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Database operation failed");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating employee");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }
    }
}
