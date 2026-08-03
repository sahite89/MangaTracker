using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MangaTracker.Application.Features.Users.RegisterUser
{
    public class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator() {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .MinimumLength(3).WithMessage("UserName must be at least 3 characters.")
                .MaximumLength(50).WithMessage("UserName must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("UserName can only contain letters, numbers, underscores and hyphens.")
                .Must(x => x == x.Trim()).WithMessage("UserName cannot start or end with spaces."); 

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(100).WithMessage("Email must not exceed 255 characters.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(x => x == x.Trim()).WithMessage("Email cannot start or end with spaces.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        }
    }
}
