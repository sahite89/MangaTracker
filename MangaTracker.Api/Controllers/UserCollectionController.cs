using MangaTracker.Api.Models;
using MangaTracker.Application.Features.UserCollections.CreateUserCollection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MangaTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCollectionController : ControllerBase
    {

        private readonly CreateUserCollectionCommandHandler _createUserCollectionHandler; 

        public UserCollectionController(CreateUserCollectionCommandHandler createUserCollectionHandler)
        {
            _createUserCollectionHandler = createUserCollectionHandler;
        }

        [Authorize]
        [HttpGet("listCollection")]
        public async Task<IActionResult> GetAllCollectionByUser()
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPost("createCollection")]
        public async Task<IActionResult> createCollection(CreateUserCollectionRequest request, CancellationToken cancellationToken)
        {
            var createUserCollectionCommand = new CreateUserCollectionCommand
            {
                UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
                MangaId = request.mangaId,
                CreatedAt = DateTime.UtcNow,
            };

            var userCollection = await _createUserCollectionHandler.HandleAsync(createUserCollectionCommand, cancellationToken);
            if (!userCollection.Success)
            {
                return BadRequest(new
                {
                    userCollection.Success,
                    userCollection.Message,
                });
            }

            return Ok(userCollection);
        }
        
    }
}
