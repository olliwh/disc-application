using backend_disc.Dtos.Companies;
using backend_disc.Repositories;
using backend_disc.Services;
using class_library_disc.Models;
using Microsoft.AspNetCore.Http;
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
