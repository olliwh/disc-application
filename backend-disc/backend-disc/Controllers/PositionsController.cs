using backend_disc.Dtos.Positions;
using backend_disc.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : GenericController<PositionDto, CreatePositionDto, UpdatePositionDto>
    {
        public PositionsController(IGenericService<PositionDto, CreatePositionDto, UpdatePositionDto> service) : base(service) { }

    }
}
