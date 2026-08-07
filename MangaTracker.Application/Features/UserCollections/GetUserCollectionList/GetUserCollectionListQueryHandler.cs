using MangaTracker.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollections.GetUserCollectionList
{
    public class GetUserCollectionListQueryHandler
    {
        private readonly IUserCollectionRepository userCollectionRepository;
        
        public GetUserCollectionListQueryHandler(IUserCollectionRepository userCollectionRepository)
        {
            this.userCollectionRepository = userCollectionRepository;
        }

        public async Task<GetUserCollectionListQueryResponse> HandleAsync(GetUserCollectionListQuery query, CancellationToken cancellationToken)
        {
            var response = new GetUserCollectionListQueryResponse();
            var userCollections = await userCollectionRepository.GetAllCollectionByUserIdAsync(query.UserId);

            if (!userCollections.Any()) { 
                response.Success = true;
                response.Message = "No collections found for the user.";
                response.listUserCollections = new List<GetUserCollectionListDto>();
                return response;
            }

            response.Success = true;
            var listUserCollectionsDto = userCollections.Select(x => new GetUserCollectionListDto
            {
                MangaId = x.MangaId,
                Title = x.Manga.Title,
                Publisher = x.Manga.Publisher,
                CoverUrl = x.Manga.CoverUrl,
                OwnedVolumes = x.Volumes.Count,
                TotalVolumes = x.Manga.TotalVolumes,
                CompletionPercentage = x.Manga.TotalVolumes > 0 ? (decimal)x.Volumes.Count / x.Manga.TotalVolumes * 100 : 0,
                Volumes = x.Volumes.Select(v => v.VolumeNumber).OrderBy(v => v).ToList()
            }).ToList();

            response.listUserCollections = listUserCollectionsDto; 

            return response;
        }
    }
}
