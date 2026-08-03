using MangaTracker.Api.Models;
using MangaTracker.Application.Features.Users.RegisterUser;
using MangaTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace MangaTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly RegisterUserCommandHandler _registerUserCommandHandler;

        public AuthController(RegisterUserCommandHandler registerUserCommandHandler)
        {
            _registerUserCommandHandler = registerUserCommandHandler;
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
        public async Task<IActionResult> Login()
        {
            throw new NotImplementedException();
        }
    }
}
