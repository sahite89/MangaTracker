using MangaTracker.Application.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Users.RegisterUser
{
    public class RegisterUserCommandResponse: BaseResponse
    {
        public RegisterUserCommandResponse() : base()
        {
        }

        public RegisterUserDto registerUserDto { get; set; } = default!;
    }
}
