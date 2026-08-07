using MangaTracker.Application.Responses;
using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollections.GetUserCollectionList
{
    public class GetUserCollectionListQueryResponse : BaseResponse
    {
        public GetUserCollectionListQueryResponse() { }

        public List<GetUserCollectionListDto> listUserCollections { get; set; } = new List<GetUserCollectionListDto>();
    }
}
