using MangaTracker.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.UserCollections.CreateUserCollection
{
    public class CreateUserCollectionCommandResponse: BaseResponse
    {
        public CreateUserCollectionCommandResponse() { }

        public CreateUserCollectionDto createUserCollectionDto { get; set; }
    }
}
