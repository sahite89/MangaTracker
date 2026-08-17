using Azure;
using MangaTracker.Api.Models.UserCollection;
using MangaTracker.Api.Models.UserCollectionVolume;
using MangaTracker.Application.Errors;
using MangaTracker.Application.Features.UserCollections.CreateUserCollection;
using MangaTracker.Application.Features.UserCollections.DeleteUserCollection;
using MangaTracker.Application.Features.UserCollections.GetUserCollectionList;
using MangaTracker.Application.Features.UserCollectionVolumenes.CreateUserCollectionVolume;
using MangaTracker.Application.Features.UserCollectionVolumenes.DeleteUserCollectionVolume;
using MangaTracker.Application.Features.UserCollectionVolumenes.GetUserCollectionVolume;
using MangaTracker.Domain.Entities;
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
        private readonly GetUserCollectionVolumeQueryHandler _getUserCollectionVolumeQueryHandler;
        private readonly DeleteUserCollectionVolumeCommandHandler _deleteUserCollectionVolumeCommandHandler;

        public UserCollectionController(CreateUserCollectionCommandHandler createUserCollectionHandler,
                                        GetUserCollectionListQueryHandler getUserCollectionListHandler,
                                        DeleteUserCollectionCommandHandler deleteUserCollectionCommandHandler,
                                        CreateUserCollectionVolumeCommandHandler createUserCollectionVolumeHandler,
                                        GetUserCollectionVolumeQueryHandler getUserCollectionVolumeHandler,
                                        DeleteUserCollectionVolumeCommandHandler deleteUserCollectionVolumeCommandHandler)
        {
            _createUserCollectionHandler = createUserCollectionHandler;
            _getUserCollectionListHandler = getUserCollectionListHandler;
            _deleteUserCollectionCommandHandler = deleteUserCollectionCommandHandler;
            _createUserCollectionVolumeHandler = createUserCollectionVolumeHandler;
            _getUserCollectionVolumeQueryHandler = getUserCollectionVolumeHandler;
            _deleteUserCollectionVolumeCommandHandler = deleteUserCollectionVolumeCommandHandler;
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

            var response = await _createUserCollectionHandler.HandleAsync(createUserCollectionCommand, cancellationToken);
            if (!response.Success)
            {
                return response.ErrorCode switch
                {
                    ErrorCode.CollectionAlreadyExists => Conflict(response),
                    ErrorCode.MangaNotFound => NotFound(response),
                    _ => BadRequest(response)
                };
            }

            return Created($"/api/userCollections/{request.mangaId}", response);
        }

        [Authorize]
        [HttpDelete("{mangaId}")]
        public async Task<IActionResult> DeleteCollection(Guid mangaId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var deleteUserCollectionCommand = new DeleteUserCollectionCommand(userId, mangaId);

            var response = await _deleteUserCollectionCommandHandler.HandleAsync(deleteUserCollectionCommand, cancellationToken);
            if (!response.Success)
            {
                return response.ErrorCode switch
                {
                    ErrorCode.CollectionNotFound => NotFound(response),
                    _ => BadRequest(response)
                };
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("{mangaId}/volumes")]
        public async Task<IActionResult> GetCollectionVolumes(Guid mangaId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var getUserCollectionVolumesQuery = new GetUserCollectionVolumeQuery
            {
                UserId = userId,
                MangaId = mangaId
            };

            var response = await _getUserCollectionVolumeQueryHandler.HandleAsync(getUserCollectionVolumesQuery,cancellationToken);
            if (!response.Success)
            {
                return response.ErrorCode switch
                {
                    ErrorCode.CollectionNotFound => NotFound(response),
                    _ => BadRequest(response)
                };

            }
            return Ok(response);
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
                return response.ErrorCode switch
                {
                    ErrorCode.ValidationError => BadRequest(response),
                    ErrorCode.MangaNotFound => NotFound(response),
                    ErrorCode.CollectionNotFound => NotFound(response),
                    ErrorCode.VolumeAlreadyExists => Conflict(response),
                    _ => BadRequest(response)
                };
            }

            return Created($"/api/userCollectionVolumes/{response.createUserCollectionVolumeDto.VolumeNumber}", response);
        }

        [Authorize]
        [HttpDelete("{mangaId}/volumes/{volumeNumber}")]
        public async Task<IActionResult> DeleteCollectionVolume(Guid mangaId, int volumeNumber, CancellationToken cancellationToken) {

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var deleteUserCollectionVolume = new DeleteUserCollectionVolumeCommand(userId, mangaId, volumeNumber);

            var response = await _deleteUserCollectionVolumeCommandHandler.HandleAsync(deleteUserCollectionVolume, cancellationToken);

            if (!response.Success)
            {
                return response.ErrorCode switch
                {
                    ErrorCode.CollectionNotFound => NotFound(response),
                    ErrorCode.VolumeNotFound => NotFound(response),
                    _ => BadRequest(response),
                };
            }

            return NoContent();
        }

    }
}
