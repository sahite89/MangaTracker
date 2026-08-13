using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Contracts.Persistence
{
    public interface IMangaRepository: IAsyncRepository<Manga>
    {
        Task<List<Manga>> GetAllAsync(string title, string Publisher);
    }
}
