using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet("getDefaultPass")]
        public ActionResult<string> getDefaultPass()
        {
            string password = "Pass@word1";
            string hash = Argon2.Hash(password);
            return Ok(hash);
        }
    }
}
