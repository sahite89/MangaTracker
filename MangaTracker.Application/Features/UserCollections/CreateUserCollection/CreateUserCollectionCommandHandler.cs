using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollections.CreateUserCollection
{
    public class CreateUserCollectionCommandHandler
    {
        private readonly IUserCollectionRepository _userCollectionRepository;

        public CreateUserCollectionCommandHandler(IUserCollectionRepository userCollectionRepository)
        {
            _userCollectionRepository = userCollectionRepository;
        }

        public async Task<CreateUserCollectionCommandResponse> HandleAsync(CreateUserCollectionCommand userCollections, CancellationToken cancellationToken)
        {

            var response = new CreateUserCollectionCommandResponse();

            // Check if exist collection in user
            var existCollection = await _userCollectionRepository.GetAsync(userCollections.UserId, userCollections.MangaId);

            if (existCollection != null) {

                response.Success = false;
                response.Message = "Existing Collection in user";

                return response;
            }

            // Add collection
            var newCollection = new UserCollection(userCollections.UserId, userCollections.MangaId);
            var insertCollection = await _userCollectionRepository.AddAsync(newCollection);

            if (insertCollection != null) {
                response.Success = true;
                response.Message = "Created Collection";
                response.createUserCollectionDto = new CreateUserCollectionDto
                {
                    userId = insertCollection.UserId,
                    mangaId = insertCollection.MangaId,
                    createdAt = insertCollection.CreatedAt,
                    volumes = insertCollection.Volumes
                };
            }

            return response;
            
        }
    }

    
}
