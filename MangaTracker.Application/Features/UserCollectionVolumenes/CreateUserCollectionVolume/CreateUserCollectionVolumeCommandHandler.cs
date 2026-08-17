using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollectionVolumenes.CreateUserCollectionVolume
{
    public class CreateUserCollectionVolumeCommandHandler
    {
        private readonly IUserCollectionVolumeRepository _userCollectionVolumeRepository;
        private readonly IUserCollectionRepository _userCollectionRepository;
        private readonly IMangaRepository _mangaRepository;

        public CreateUserCollectionVolumeCommandHandler(IUserCollectionVolumeRepository userCollectionVolumeRepository,
                                                        IUserCollectionRepository userCollectionRepository,
                                                        IMangaRepository mangaRepository)
        {
            _userCollectionVolumeRepository = userCollectionVolumeRepository;
            _userCollectionRepository = userCollectionRepository;
            _mangaRepository = mangaRepository;
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
                response.Message = "Error to create volume";
                response.ErrorCode = ErrorCode.ValidationError;
                return response;
            }

            var existManga = await _mangaRepository.GetByIdAsync(userCollectionVolume.MangaId);
            if (existManga == null)
            {
                response.Success = false;
                response.Message = "Manga not found";
                response.ErrorCode = ErrorCode.MangaNotFound;
                return response;
            }

            var existCollection = await _userCollectionRepository.GetAsync(userCollectionVolume.UserId, userCollectionVolume.MangaId);
            if (existCollection == null)
            {
                response.Success = false;
                response.Message = "Collection not found";
                response.ErrorCode = ErrorCode.CollectionNotFound;
                return response;
            }

            var existVolume = await _userCollectionVolumeRepository.GetUserCollectionVolume(userCollectionVolume.UserId, userCollectionVolume.MangaId, userCollectionVolume.VolumeNumber);
            if (existVolume != null)
            {
                response.Success = false;
                response.Message = "Existing Volume in collection";
                response.ErrorCode = ErrorCode.VolumeAlreadyExists;
                return response;
            }

            var newVolume = new Domain.Entities.UserCollectionVolume(userCollectionVolume.UserId, userCollectionVolume.MangaId, userCollectionVolume.VolumeNumber);
            var insertVolume = await _userCollectionVolumeRepository.AddAsync(newVolume);
            
            response.Message = "Volume Created";
            response.createUserCollectionVolumeDto = new CreateUserCollectionVolumeDto
            {
                UserId = insertVolume.UserId,
                MangaId = insertVolume.MangaId,
                VolumeNumber = insertVolume.VolumeNumber
            };

            return response;
        }
    }
}
