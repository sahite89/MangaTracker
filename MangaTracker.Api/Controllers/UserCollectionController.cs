using MangaTracker.Api.Models.UserCollection;
using MangaTracker.Api.Models.UserCollectionVolume;
using MangaTracker.Application.Features.UserCollections.CreateUserCollection;
using MangaTracker.Application.Features.UserCollections.DeleteUserCollection;
using MangaTracker.Application.Features.UserCollections.GetUserCollectionList;
using MangaTracker.Application.Features.UserCollectionVolumenes.CreateUserCollectionVolume;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MangaTracker.Api.Controllers
{
    [ApiController]
    [Route("api/user-collections")]
    public class UserCollectionController : ControllerBase
    {

        private readonly CreateUserCollectionCommandHandler _createUserCollectionHandler;
        private readonly GetUserCollectionListQueryHandler _getUserCollectionListHandler;
        private readonly DeleteUserCollectionCommandHandler _deleteUserCollectionCommandHandler;
        private readonly CreateUserCollectionVolumeCommandHandler _createUserCollectionVolumeHandler;

        public UserCollectionController(CreateUserCollectionCommandHandler createUserCollectionHandler,
                                        GetUserCollectionListQueryHandler getUserCollectionListHandler,
                                        DeleteUserCollectionCommandHandler deleteUserCollectionCommandHandler,
                                        CreateUserCollectionVolumeCommandHandler createUserCollectionVolumeHandler)
        {
            _createUserCollectionHandler = createUserCollectionHandler;
            _getUserCollectionListHandler = getUserCollectionListHandler;
            _deleteUserCollectionCommandHandler = deleteUserCollectionCommandHandler;
            _createUserCollectionVolumeHandler = createUserCollectionVolumeHandler;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCollection(CancellationToken cancellationToken)
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
        [HttpPost]
        public async Task<IActionResult> CreateCollection(CreateUserCollectionRequest request, CancellationToken cancellationToken)
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

        [Authorize]
        [HttpDelete("{mangaId}")]
        public async Task<IActionResult> DeleteCollection(Guid mangaId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var deleteUserCollectionCommand = new DeleteUserCollectionCommand(userId, mangaId);

            var userCollection = await _deleteUserCollectionCommandHandler.HandleAsync(deleteUserCollectionCommand, cancellationToken);
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

        [Authorize]
        [HttpGet("{mangaId}/volumes")]
        public async Task<IActionResult> GetCollectionVolumes(Guid mangaId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            //var getUserCollectionVolumesQuery = new GetUserCollectionVolumesQuery
            //{
            //    UserId = userId,
            //    MangaId = mangaId
            //};
            //var userCollectionVolumes = await _getUserCollectionListHandler.HandleAsync(getUserCollectionVolumesQuery, cancellationToken);
            //if (!userCollectionVolumes.Success)
            //{
            //    return BadRequest(new
            //    {
            //        userCollectionVolumes.Success,
            //        userCollectionVolumes.Message,
            //    });
            //}
            //return Ok(userCollectionVolumes);
        }

        [Authorize]
        [HttpPost("{mangaId}/volumes")]
        public async Task<IActionResult> CreateCollectionVolume(Guid mangaId, CreateUserCollectionVolumeRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateUserCollectionVolumeCommand
            {
                UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
                MangaId = mangaId,
                VolumeNumber = request.volumenNumber
            };
            var response = await _createUserCollectionVolumeHandler.HandleAsync(command, cancellationToken);

            if (!response.Success)
            {
                return BadRequest(new
                {
                    response.Message,
                    response.ValidationErrors
                });
            }

            return Created($"/api/userCollectionVolumes/{response.createUserCollectionVolumeDto.VolumeNumber}", response);
        }
    }
}
