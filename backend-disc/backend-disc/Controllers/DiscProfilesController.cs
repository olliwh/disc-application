using backend_disc.Dtos.DiscProfiles;
using backend_disc.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscProfilesController : GenericController<DiscProfileDto, CreateDiscProfileDto, UpdateDiscProfileDto>
    {
        public DiscProfilesController(IGenericService<DiscProfileDto, CreateDiscProfileDto, UpdateDiscProfileDto> service) : base(service) { }

    }
}
