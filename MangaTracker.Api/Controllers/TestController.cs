using Microsoft.AspNetCore.Mvc;
using MangaTracker.Infrastructure;
using MangaTracker.Infrastructure.Persistence;
using MangaTracker.Domain.Entities;
namespace MangaTracker.Api.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("MangaTracker funcionando.");
        }
    }
}