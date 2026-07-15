using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Users.RegisterUser
{
    public class RegisterUserCommand 
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
