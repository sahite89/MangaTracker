using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Users.LoginUser
{
    public class LoginUserCommand
    {
        public string Email { get; set;  }
        public string Password { get; set; }    
    }
}
