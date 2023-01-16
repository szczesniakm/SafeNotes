using FluentValidation;
using SafeNotes.Application.Extensions;

namespace SafeNotes.Application.Models.Notes
{
    public record GetNoteModel(int NoteId, string? Key) : GetNoteRequest(Key);

    public class GetNoteRequestValidator : AbstractValidator<GetNoteModel>
    {
        public GetNoteRequestValidator()
        {
            RuleFor(x => x.Key)
             .MinimumLength(12)
             .MaximumLength(320)
             .Must(x => x.ContainsLetterCharacter()).WithMessage("Password must contain at least one letter.")
             .Must(x => x.ContainsDigitsCharacter()).WithMessage("Password must contain at least one digit.")
             .Must(x => x.ContainsSpecialCharacter()).WithMessage("Password must contain at least one special character.")
             .When(x => x.Key is not null);
        }
    }
}
