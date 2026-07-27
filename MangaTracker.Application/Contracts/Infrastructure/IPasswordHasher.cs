using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Contracts.Infrastructure
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(
            string password,
            string hash);
    }
}
