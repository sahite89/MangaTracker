using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Errors;
using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollections.CreateUserCollection
{
    public class CreateUserCollectionCommandHandler
    {
        private readonly IUserCollectionRepository _userCollectionRepository;
        private readonly IMangaRepository _mangaRepository;

        public CreateUserCollectionCommandHandler(IUserCollectionRepository userCollectionRepository, IMangaRepository mangaRepository)
        {
            _userCollectionRepository = userCollectionRepository;
            _mangaRepository = mangaRepository;
        }

        public async Task<CreateUserCollectionCommandResponse> HandleAsync(CreateUserCollectionCommand userCollections, CancellationToken cancellationToken)
        {
            var response = new CreateUserCollectionCommandResponse();

            var existManga = await _mangaRepository.GetByIdAsync(userCollections.MangaId);
            if (existManga == null)
            {
                response.Success = false;
                response.Message = "Manga not Found";
                response.ErrorCode = ErrorCode.MangaNotFound;
                return response;
            }

            var existCollection = await _userCollectionRepository.GetAsync(userCollections.UserId, userCollections.MangaId);

            if (existCollection != null)
            {

                response.Success = false;
                response.Message = "Existing Collection in user";
                response.ErrorCode = ErrorCode.CollectionAlreadyExists;
                return response;
            }

            var newCollection = new UserCollection(userCollections.UserId, userCollections.MangaId);
            var insertCollection = await _userCollectionRepository.AddAsync(newCollection);

            response.Message = "Created Collection";
            response.createUserCollectionDto = new CreateUserCollectionDto
            {
                userId = insertCollection.UserId,
                mangaId = insertCollection.MangaId,
                createdAt = insertCollection.CreatedAt,
                volumes = insertCollection.Volumes
            };


            return response;

        }
    }


}
