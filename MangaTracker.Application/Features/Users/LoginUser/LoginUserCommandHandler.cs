using MangaTracker.Application.Contracts.Infrastructure;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Users.LoginUser
{
    public class LoginUserCommandHandler
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserRepository _userRepository;

        public LoginUserCommandHandler(IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _userRepository = userRepository;
        }

        public async Task<LoginUserCommandResponse> HandleAsync(LoginUserCommand loginUser, CancellationToken cancellationToken)
        {
            var response = new LoginUserCommandResponse();
            var validator = new LoginUserCommandValidator();

            var responseValidator = validator.Validate(loginUser);
            if (!responseValidator.IsValid)
            {
                response.Success = false;
                response.ErrorCode = ErrorCode.ValidationError;
                response.Message = "Validation errors occurred";
                response.ValidationErrors = responseValidator.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }

            var user = await _userRepository.GetUserByEmailAsync(loginUser.Email);

            if (user == null) { 
                response.Success = false;
                response.Message = "Invalid email or password";
                response.ErrorCode = ErrorCode.InvalidCredentials;
                return response;
            }

            var checkPassword = _passwordHasher.Verify(loginUser.Password, user.PasswordHash);
            
            if (!checkPassword)
            {
                response.Success = false;
                response.Message = "Invalid email or password";
                response.ErrorCode = ErrorCode.InvalidCredentials;

                return response;
            }

            var token = _jwtProvider.GenerateToken(user);
            response.loginUserDto = new LoginUserDto
            {
                Token = token,
                UserId = user.Id,
                UserName = user.UserName
            };

            return response;
        }
    }
}
