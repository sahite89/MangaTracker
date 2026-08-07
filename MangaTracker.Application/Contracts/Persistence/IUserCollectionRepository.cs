using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Contracts.Persistence
{
    public interface IUserCollectionRepository : IAsyncRepository<UserCollection>
    {
        Task<UserCollection?> GetAsync(Guid userId, Guid mangaId);
        Task<List<UserCollection>> GetAllCollectionByUserIdAsync(Guid userId);
    }
}
