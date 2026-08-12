using MangaTracker.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollectionVolumenes.GetUserCollectionVolume
{
    public class GetUserCollectionVolumeQueryHandler
    {
        private readonly IUserCollectionVolumeRepository _userCollectionVolumeRepository;
        private readonly IUserCollectionRepository _userCollectionRepository;

        public GetUserCollectionVolumeQueryHandler(IUserCollectionVolumeRepository userCollectionVolumeRepository, IUserCollectionRepository userCollectionRepository)
        {
            _userCollectionVolumeRepository = userCollectionVolumeRepository;
            _userCollectionRepository = userCollectionRepository;
        }

        public async Task<GetUserCollectionVolumeQueryResponse> HandleAsync(GetUserCollectionVolumeQuery query, CancellationToken cancellationToken)
        {
            var response = new GetUserCollectionVolumeQueryResponse();

            var existcollection = await _userCollectionRepository.GetAsync(query.UserId, query.MangaId);

            if(existcollection == null)
            {
                response.Success = false;
                response.Message = "User collection not found.";
                return response;
            }

            var userCollectionVolume = await _userCollectionVolumeRepository.GetUserCollectionVolumes(query.UserId, query.MangaId);
            if (userCollectionVolume == null)
            {
                response.Success = false;
                response.Message = "User collection volume not found.";
                return response;
            }

            response.Success = true;

            response.getUserCollectionVolumeDto = new GetUserCollectionVolumeDto
            {
                VolumeNumbers = userCollectionVolume
                                    .Select(v => v.VolumeNumber)
                                    .ToList(),
            };

            return response;
        }
    }
}
