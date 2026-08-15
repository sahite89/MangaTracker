using MangaTracker.Application.Contracts.Infrastructure;
using MangaTracker.Application.Contracts.Persistence;
using MangaTracker.Application.Errors;
using MangaTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Users.RegisterUser
{
    public class RegisterUserCommandHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegisterUserCommandResponse> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var response = new RegisterUserCommandResponse();
            var validationResult = new RegisterUserCommandValidator().Validate(request);

            if (validationResult.Errors.Count > 0) {
                response.Success = false;
                response.ErrorCode = ErrorCode.ValidationError;
                response.Message = "Validation errors occurred";
                response.ValidationErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return response;
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                response.Success = false;
                response.Message = "User already exists";
                response.ErrorCode = ErrorCode.UserAlreadyExists;
                return response;
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };

            var createdUser = await _userRepository.AddAsync(newUser);

            response.RegisterUserDto = new RegisterUserDto
            {
                UserId = createdUser.Id,
                UserName = createdUser.UserName
            };
            return response;
        }
    }
}
