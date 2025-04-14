using CountryAPP.Core.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryAPP.Core.Validator;

    public class UserCreateModelValidator : AbstractValidator<UserModel>
    {
        public UserCreateModelValidator()
        {
            RuleFor(x => x.Email)
             .NotEmpty().WithMessage("Email is required.")
             .EmailAddress().WithMessage("Invalid email format.")
             .MaximumLength(100);

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MinimumLength(3).WithMessage("Full name must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Full name cannot exceed 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(50).WithMessage("Password cannot exceed 50 characters.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{11}$").WithMessage("Phone number must be 11 digits.");
        }
    }
