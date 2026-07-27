using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Contracts.Persistence
{
    public interface IUserRepository: IAsyncRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
