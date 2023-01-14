using FluentValidation;
using SafeNotes.Application.Extensions;
using SafeNotes.Domain.Repositories;
using static SafeNotes.Application.Models.Notes.UpdateAllowedUsersRequest;

namespace SafeNotes.Application.Models.Notes
{
    public record UpdateAllowedUsersRequest(int NoteId, IEnumerable<UserAccess> AllowedUsers)
    {
        public record UserAccess(string? Email, bool? CanRead, bool? CanWrite);
    }

    public class UpdateAllowedUsersRequestValidator : AbstractValidator<UpdateAllowedUsersRequest>
    {
        public UpdateAllowedUsersRequestValidator(INotesRepository notesRepository)
        {
            RuleFor(x => x.NoteId)
                .NotEmpty()
                .MustAsync(async (id, cancellation) => await notesRepository.Exists(id, cancellation))
                .WithMessage("Note does not exists.");

            RuleForEach(x => x.AllowedUsers).ChildRules(allowedUser =>
            {
                allowedUser
                    .RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(320)
                    .Must(x => x.DoesNotContainDangerousCharacters()).WithMessage("Email contains forbidden characters.");

                allowedUser
                    .RuleFor(x => x.CanRead)
                    .NotNull();

                allowedUser
                    .RuleFor(x => x.CanWrite)
                    .NotNull();
            });
        }
    }
}
