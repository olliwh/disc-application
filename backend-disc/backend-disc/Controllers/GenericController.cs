using backend_disc.Dtos.BaseDtos;
using backend_disc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    /// <summary>
    /// BaseController
    /// Abstract because it should never be used directly
    /// All functions can be overwridden because of virtual
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericController<TDto, TCreateDto, TUpdateDto> : ControllerBase
        where TDto : BaseDto
        where TCreateDto : ICreateDtoBase
        where TUpdateDto : IUpdateDtoBase
    {
        private readonly IGenericService<TDto, TCreateDto, TUpdateDto> _service;


        protected GenericController(IGenericService<TDto, TCreateDto, TUpdateDto> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> GetAll([FromQuery] int? departmentId = null,
            [FromQuery] string db = "mssql",
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10)
        {
            var entities = await _service.GetAllAsync(pageIndex, pageSize, db);
            return Ok(entities);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> GetById(int id, [FromQuery] string db = "mssql")
        {
            var dto = await _service.GetByIdAsync(id, db);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//not admin role
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//in valid token
        [Authorize(Roles = "Admin,Manager")]
        public virtual async Task<IActionResult> Create([FromBody] TCreateDto createDto, [FromQuery] string db = "mssql")
        {
            try
            {
                var created = await _service.CreateAsync(createDto, db);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//not admin role
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//in valid token
        [Authorize(Roles = "Admin,Manager")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] TUpdateDto updateDto, [FromQuery] string db = "mssql")
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateDto, db);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//not admin role
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//in valid token
        [Authorize(Roles = "Admin")]
        public virtual async Task<IActionResult> Delete(int id, [FromQuery] string db = "mssql")
        {
            var deleted = await _service.DeleteAsync(id, db);
            if (deleted == null) return NotFound();
            return Ok(deleted);
        }
    }
}
