using Azure;
using MangaTracker.Api.Models.User;
using MangaTracker.Application.Errors;
using MangaTracker.Application.Features.Users.LoginUser;
using MangaTracker.Application.Features.Users.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace MangaTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly RegisterUserCommandHandler _registerUserCommandHandler;
        private readonly LoginUserCommandHandler _loginUserCommandHandler;

        public AuthController(RegisterUserCommandHandler registerUserCommandHandler, LoginUserCommandHandler loginUserCommandHandler)
        {
            _registerUserCommandHandler = registerUserCommandHandler;
            _loginUserCommandHandler = loginUserCommandHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest user, CancellationToken cancellationToken)
        {
            var registerUserCommand = new RegisterUserCommand
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password
            };

            var response = await _registerUserCommandHandler.HandleAsync(registerUserCommand, cancellationToken);
            if (!response.Success)
            {
                return response.ErrorCode switch
                {
                    ErrorCode.ValidationError => BadRequest(response),
                    ErrorCode.UserAlreadyExists => Conflict(response),
                    _ => BadRequest(response)
                };
            }

            return Created($"/api/users/{response.RegisterUserDto!.UserId}", response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest loginUser, CancellationToken cancellationToken)
        {
            var loginUserCommand = new LoginUserCommand
            {
                Email = loginUser.Email,
                Password = loginUser.Password
            };

            var response = await _loginUserCommandHandler.HandleAsync(loginUserCommand, cancellationToken);
            
            if(!response.Success)
            {
                return response.ErrorCode switch
                {
                    ErrorCode.InvalidCredentials => Unauthorized(response),
                    ErrorCode.ValidationError => BadRequest(response),
                    _ => BadRequest(response)
                };
            }

            return Ok(response);
        }
    }
}
