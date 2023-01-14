using FluentValidation;
using SafeNotes.Application.Authorization;
using SafeNotes.Application.Models;
using SafeNotes.Application.Models.Notes;
using SafeNotes.Domain.Entities;
using SafeNotes.Domain.Repositories;

namespace SafeNotes.Application.Services
{
    public class NoteService
    {
        private readonly INotesRepository _notesRepository;
        private readonly IValidator<CreateNoteRequest> _createNoteRequestValidator;
        private readonly IUserIdentityProvider _userIdentityProvider;

        public NoteService(
            INotesRepository notesRepository,
            IValidator<CreateNoteRequest> createNoteRequestValidator,
            IUserIdentityProvider userIdentityProvider)
        {
            _notesRepository = notesRepository;
            _createNoteRequestValidator = createNoteRequestValidator;
            _userIdentityProvider = userIdentityProvider;
        }

        public async Task<Result<IdModel>> CreateNote(CreateNoteRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _createNoteRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ValidationException(validationResult.Errors);
            }

            var user = await _userIdentityProvider.GetUser(cancellationToken);

            var note = new Note(
                user,
                request.Title!,
                request.Content!,
                request.IsPublic!.Value,
                request.IsEncryptedWithUserSpecifiedKey!.Value);

            await _notesRepository.Add(note, cancellationToken);

            return new IdModel(note.Id);
        }
    }
}
