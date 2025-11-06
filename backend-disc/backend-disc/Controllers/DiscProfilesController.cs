using backend_disc.Repositories;
using class_library_disc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscProfilesController : GenericController<DiscProfile>
    {
        public DiscProfilesController(IGenericRepository<DiscProfile> repository) : base(repository) { }

    }
}
