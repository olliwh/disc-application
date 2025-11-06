using backend_disc.Repositories;
using class_library_disc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : GenericController<Position>
    {
        public PositionsController(IGenericRepository<Position> repository) : base(repository) { }

    }
}
