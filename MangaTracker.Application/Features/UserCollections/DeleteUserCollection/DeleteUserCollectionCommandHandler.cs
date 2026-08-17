using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Errors;
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

            var existCollection = await _userCollectionRepository.GetAsync(userCollections.UserId, userCollections.MangaId);
            if (existCollection == null)
            {
                response.Success = false;
                response.Message = "Collection not found";
                response.ErrorCode = ErrorCode.CollectionNotFound;
                return response;
            }

            await _userCollectionRepository.DeleteAsync(existCollection);

            return response;
        }
    }
}
