using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Users.RegisterUser
{
    public class RegisterUserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
