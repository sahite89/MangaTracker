using MangaTracker.Api.Models.UserCollection;
using MangaTracker.Application.Features.UserCollections.CreateUserCollection;
using MangaTracker.Application.Features.UserCollections.GetUserCollectionList;
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
        private readonly GetUserCollectionListQueryHandler _getUserCollectionListHandler;

        public UserCollectionController(CreateUserCollectionCommandHandler createUserCollectionHandler, GetUserCollectionListQueryHandler getUserCollectionListHandler)
        {
            _createUserCollectionHandler = createUserCollectionHandler;
            _getUserCollectionListHandler = getUserCollectionListHandler;
        }

        [Authorize]
        [HttpGet("listCollection")]
        public async Task<IActionResult> GetAllCollectionByUser(CancellationToken cancellationToken)
        {
            var userCollectionListQuery = new GetUserCollectionListQuery
            {
                UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
            };

            var userCollectionList = await _getUserCollectionListHandler.HandleAsync(userCollectionListQuery, cancellationToken);

            if (!userCollectionList.Success)
            {
                return BadRequest(new
                {
                    userCollectionList.Success,
                    userCollectionList.Message,
                });
            }
            ;

            return Ok(userCollectionList);

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

            return Created($"/api/userCollections/{userCollection.createUserCollectionDto}", userCollection);
        }

    }
}
