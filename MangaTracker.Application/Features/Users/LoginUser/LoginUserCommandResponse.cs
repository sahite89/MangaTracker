using MangaTracker.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Users.LoginUser
{
    public class LoginUserCommandResponse : BaseResponse
    {
        public LoginUserCommandResponse() { }

        public LoginUserDto? loginUserDto { get; set; }
    }
}
