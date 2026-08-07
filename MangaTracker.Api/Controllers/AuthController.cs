using MangaTracker.Api.Models.User;
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

            var userId = await _registerUserCommandHandler.HandleAsync(registerUserCommand, cancellationToken);
            if (!userId.Success)
            {
                return BadRequest(new
                {
                    userId.Message,
                    userId.ValidationErrors
                });
            }

            return Created($"/api/users/{userId.RegisterUserDto!.UserId}", userId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest loginUser, CancellationToken cancellationToken)
        {
            var loginUserCommand = new LoginUserCommand
            {
                Email = loginUser.Email,
                Password = loginUser.Password
            };

            var userLoged = await _loginUserCommandHandler.HandleAsync(loginUserCommand, cancellationToken);
            
            if(!userLoged.Success)
            {
                return BadRequest(new
                {
                    userLoged.Message,
                });
            }

            return Ok(userLoged);
        }
    }
}
