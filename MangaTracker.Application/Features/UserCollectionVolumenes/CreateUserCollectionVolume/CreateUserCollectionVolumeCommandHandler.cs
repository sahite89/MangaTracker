using MangaTracker.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollectionVolumenes.CreateUserCollectionVolume
{
    public class CreateUserCollectionVolumeCommandHandler
    {
        private readonly IUserCollectionVolumeRepository _userCollectionVolumeRepository;
        private readonly IUserCollectionRepository _userCollectionRepository;

        public CreateUserCollectionVolumeCommandHandler(IUserCollectionVolumeRepository userCollectionVolumeRepository, IUserCollectionRepository userCollectionRepository)
        {
            _userCollectionVolumeRepository = userCollectionVolumeRepository;
            _userCollectionRepository = userCollectionRepository;
        }

        public async Task<CreateUserCollectionVolumeCommandResponse> HandleAsync(CreateUserCollectionVolumeCommand userCollectionVolume, CancellationToken cancellationToken)
        {
            var response = new CreateUserCollectionVolumeCommandResponse();
            var validation = new CreateUserCollectionVolumeCommandValidator();

            var validationResult = validation.Validate(userCollectionVolume);
            // TODO: Check if volume number is in range of manga volumes
            if (!validationResult.IsValid)
            {
                response.ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                response.Success = false;
                response.Message = "Error to create user collection volume";
                return response;
            }

            // Check if collection exist in user collection
            var existCollection = await _userCollectionRepository.GetAsync(userCollectionVolume.UserId, userCollectionVolume.MangaId);
            if (existCollection == null)
            {
                response.Success = false;
                response.Message = "Collection not found for the specified user";
                return response;
            }

            // Check if exist volume in user collection
            var existVolume = await _userCollectionVolumeRepository.GetUserCollectionVolume(userCollectionVolume.UserId, userCollectionVolume.MangaId, userCollectionVolume.VolumeNumber);
            if (existVolume != null)
            {
                response.Success = false;
                response.Message = "Existing Volume in user collection";
                return response;
            }
            // Add volume
            var newVolume = new Domain.Entities.UserCollectionVolume(userCollectionVolume.UserId, userCollectionVolume.MangaId, userCollectionVolume.VolumeNumber);
            var insertVolume = await _userCollectionVolumeRepository.AddAsync(newVolume);
            if (insertVolume != null)
            {
                response.Success = true;
                response.Message = "Created Volume";
                response.createUserCollectionVolumeDto = new CreateUserCollectionVolumeDto
                {
                    UserId = insertVolume.UserId,
                    MangaId = insertVolume.MangaId,
                    VolumeNumber = insertVolume.VolumeNumber
                };
            }
            return response;
        }
    }
}
