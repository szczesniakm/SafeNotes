using FluentValidation;
using SafeNotes.Application.Extensions;

namespace SafeNotes.Application.Models.Notes
{
    public record CreateNoteRequest(
        string? Title,
        string? Content,
        bool? IsPublic,
        bool? IsEncryptedWithUserSpecifiedKey,
        string? Key);

    public class CreateNoteRequestValidator : AbstractValidator<CreateNoteRequest>
    {
        public CreateNoteRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(1024);

            RuleFor(x => x.Content)
                .NotEmpty();

            RuleFor(x => x.IsPublic)
                .NotEmpty();

            RuleFor(x => x.IsEncryptedWithUserSpecifiedKey)
                .NotEmpty()
                .When(x => x.IsPublic.HasValue && x.IsPublic.Value);

            RuleFor(x => x.Key)
                .NotEmpty()
                .MinimumLength(12)
                .MaximumLength(320)
                .Must(x => x.ContainsLetterCharacter()).WithMessage("Password must contain at least one letter.")
                .Must(x => x.ContainsDigitsCharacter()).WithMessage("Password must contain at least one digit.")
                .Must(x => x.ContainsSpecialCharacter()).WithMessage("Password must contain at least one special character.")
                .When(x => x.IsEncryptedWithUserSpecifiedKey.HasValue && x.IsEncryptedWithUserSpecifiedKey.Value);
        }
    }
}
