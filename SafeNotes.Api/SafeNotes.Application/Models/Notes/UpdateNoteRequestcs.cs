using FluentValidation;
using SafeNotes.Application.Extensions;

namespace SafeNotes.Application.Models.Notes
{
    public record UpdateNoteRequest(string Title, string Content, string? Key);

    public record UpdateNoteModel(int NoteId, string Title, string Content, string? Key) : UpdateNoteRequest(Title, Content, Key);

    public class UpdateNoteModelValidator : AbstractValidator<UpdateNoteModel>
    {
        public UpdateNoteModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(1024);

            RuleFor(x => x.Content)
                .NotEmpty();

            RuleFor(x => x.Key)
                .NotEmpty()
                .MinimumLength(12)
                .MaximumLength(320)
                .Must(x => x.ContainsLetterCharacter()).WithMessage("Password must contain at least one letter.")
                .Must(x => x.ContainsDigitsCharacter()).WithMessage("Password must contain at least one digit.")
                .Must(x => x.ContainsSpecialCharacter()).WithMessage("Password must contain at least one special character.")
                .When(x => x.Key is not null);
        }
    }
}
