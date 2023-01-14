using FluentValidation;
using SafeNotes.Application.Extensions;
using SafeNotes.Domain.Repositories;

namespace SafeNotes.Application.Models.Users
{
    public record RegisterUserRequest(string Email, string Password);

    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator(IUsersRepository usersRepository)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(320)
                .Must(x => x.DoesNotContainDangerousCharacters()).WithMessage("Email contains forbidden characters.")
                .MustAsync(async (email, cancellation) => !await usersRepository.Exists(email, cancellation))
                .WithMessage("User already exists.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(12)
                .MaximumLength(320)
                .Must(x => x.ContainsLetterCharacter()).WithMessage("Password must contain at least one letter.")
                .Must(x => x.ContainsDigitsCharacter()).WithMessage("Password must contain at least one digit.")
                .Must(x => x.ContainsSpecialCharacter()).WithMessage("Password must contain at least one special character.");
        }
    }
}
