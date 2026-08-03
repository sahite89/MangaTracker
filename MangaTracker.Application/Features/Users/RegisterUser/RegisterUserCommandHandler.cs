using MangaTracker.Application.Contracts.Infrastructure;
using MangaTracker.Application.Contracts.Persistence;
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
                response.ValidationErrors = new List<string>();
                validationResult.Errors.ForEach(error => response.ValidationErrors.Add(error.ErrorMessage));
                response.Message = "Validation errors occurred";
                return response;
            }
            // Check if the user already exists
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                response.Success = false;
                response.Message = "User already exists";
                return response;
            }
            // Create a new user entity
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };
            // Save the new user to the repository
            var createdUser = await _userRepository.AddAsync(newUser);
            // Prepare the response DTO
            response.RegisterUserDto = new RegisterUserDto
            {
                UserId = createdUser.Id,
                UserName = createdUser.UserName
            };
            return response;
        }
    }
}
