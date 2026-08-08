using MangaTracker.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollections.DeleteUserCollection
{
    public class DeleteUserCollectionCommandHandler
    {
        private readonly IUserCollectionRepository _userCollectionRepository;

        public DeleteUserCollectionCommandHandler(IUserCollectionRepository userCollectionRepository)
        {
            _userCollectionRepository = userCollectionRepository;
        }

        public async Task<DeleteUserCollectionCommandResponse> HandleAsync(DeleteUserCollectionCommand userCollections, CancellationToken cancellationToken)
        {
            var response = new DeleteUserCollectionCommandResponse();
            // Check if exist collection in user
            var existCollection = await _userCollectionRepository.GetAsync(userCollections.UserId, userCollections.MangaId);
            if (existCollection == null)
            {
                response.Success = false;
                response.Message = "Collection not found in user";
                return response;
            }
            // Delete collection
            await _userCollectionRepository.DeleteAsync(existCollection);

            response.Success = true;
            response.Message = "Deleted Collection: " + existCollection.MangaId;
            return response;
        }
    }
}
