using backend_disc.Repositories;
using Microsoft.AspNetCore.Http;
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
    public abstract class GenericController<TEntity> : ControllerBase where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;

        protected GenericController(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<IActionResult> GetAll(
                [FromQuery] int? departmentId = null,
                [FromQuery] int? discProfileId = null,
                [FromQuery] int? positionId = null)
        {
            var entities = await _repository.GetAll();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> Create([FromBody] TEntity entity)
        {
            var created = await _repository.Add(entity);

            var idProp = created?.GetType().GetProperty("Id")?.GetValue(created);

            return CreatedAtAction(nameof(GetById), new { id = idProp }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Update(int id, [FromBody] TEntity entity)
        {
            var updated = await _repository.Update(id, entity);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.Delete(id);
            if (deleted == null) return NotFound();
            return Ok(deleted);
        }
    }
}
