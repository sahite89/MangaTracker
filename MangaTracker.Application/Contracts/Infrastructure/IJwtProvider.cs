using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Contracts.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
