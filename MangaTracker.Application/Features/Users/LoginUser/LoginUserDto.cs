using MangaTracker.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Users.LoginUser
{
    public class LoginUserDto
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }

    }
}
