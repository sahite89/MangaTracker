using Microsoft.AspNetCore.Mvc;

namespace MangaTracker.Api.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("MangaTracker funcionando");
        }
    }
}
