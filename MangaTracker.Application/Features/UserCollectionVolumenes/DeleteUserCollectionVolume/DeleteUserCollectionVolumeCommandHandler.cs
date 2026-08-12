using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Features.UserCollections.DeleteUserCollection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollectionVolumenes.DeleteUserCollectionVolume
{
    public class DeleteUserCollectionVolumeCommandHandler
    {
        private readonly IUserCollectionVolumeRepository _userCollectionVolumeRepository;

        public DeleteUserCollectionVolumeCommandHandler(IUserCollectionVolumeRepository userCollectionVolumeRepository)
        {
            _userCollectionVolumeRepository = userCollectionVolumeRepository;
        }

        public async Task<DeleteUserCollectionVolumeCommandResponse> HandleAsync(DeleteUserCollectionVolumeCommand command, CancellationToken cancellationToken)
        {
            var response = new DeleteUserCollectionVolumeCommandResponse();

            var existCollection = await _userCollectionVolumeRepository.GetUserCollectionVolumes(command.UserId, command.MangaId);
            if(existCollection.Count == 0)
            {
                response.Success = false;
                response.Message = "Collection not found.";
                return response;
            }

            var existVolumenInCollection = await _userCollectionVolumeRepository.GetUserCollectionVolume(command.UserId, command.MangaId, command.VolumeNumber);
            if (existVolumenInCollection == null)
            {
                response.Success = false;
                response.Message = "Volume not found in collection.";
                return response;
            }

            await _userCollectionVolumeRepository.DeleteAsync(existVolumenInCollection!);

            response.Success = true;
            response.Message = "Deleted volume succeeded: " + existVolumenInCollection!.VolumeNumber ;

            return response;
        }
    }
}
