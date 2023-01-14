using FluentValidation;
using Ganss.Xss;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SafeNotes.Application.Authorization;
using SafeNotes.Application.Models;
using SafeNotes.Application.Models.Notes;
using SafeNotes.Application.Utils;
using SafeNotes.Domain.Entities;
using SafeNotes.Domain.Repositories;

namespace SafeNotes.Application.Services
{
    public class NoteService
    {
        private readonly INotesRepository _notesRepository;
        private readonly IValidator<CreateNoteRequest> _createNoteRequestValidator;
        private readonly IValidator<UpdateAllowedUsersRequest> _updateAllowedUsersRequestValidator;
        private readonly IUserIdentityProvider _userIdentityProvider;
        
        public NoteService(
            INotesRepository notesRepository,
            IValidator<CreateNoteRequest> createNoteRequestValidator,
            IValidator<UpdateAllowedUsersRequest> updateAllowedUsersRequestValidator,
            IUserIdentityProvider userIdentityProvider)
        {
            _notesRepository = notesRepository;
            _createNoteRequestValidator = createNoteRequestValidator;
            _updateAllowedUsersRequestValidator = updateAllowedUsersRequestValidator;
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

            var sanitizer = new HtmlSanitizer();

            var content = sanitizer.Sanitize(request.Content!);
            string? salt = null;

            if(!request.IsPublic!.Value && request.IsEncryptedWithUserSpecifiedKey!.Value)
            {
                (var key, salt)= EncryptionHelper.StretchKey(request.Key!);
                content = EncryptionHelper.Encrypt(content, key);
            }

             var note = new Note(
                    user,
                    request.Title!,
                    content,
                    request.IsPublic!.Value,
                    request.IsEncryptedWithUserSpecifiedKey!.Value,
                    salt);


            await _notesRepository.Add(note, cancellationToken);

            return new IdModel(note.Id);
        }

        public async Task<Result> UpdateAllowedUsers(UpdateAllowedUsersRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _updateAllowedUsersRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ValidationException(validationResult.Errors);
            }

            var user = await _userIdentityProvider.GetUser(cancellationToken);

            var note = await _notesRepository.GetById(request.NoteId, cancellationToken);

            if(note!.OwnerEmail != user.Email)
            {
                return new Exception("Not Found");
            }

            var allowedUsers = request.AllowedUsers.Select(x => new UserAccess(x.Email!, x.CanRead!.Value, x.CanWrite!.Value)).ToList();

            note!.UpdateAllowedUsers(allowedUsers);

            await _notesRepository.Update(note, cancellationToken);

            return Result.Ok;
        }

        public string DecryptNote(Note note, string key, CancellationToken cancellationToken)
        {
            if (!note.IsPublic && note.IsEncryptedWithUserSpecifiedKey)
            {
                var keyBytes = EncryptionHelper.HashKey(key, note.Salt!);
                return EncryptionHelper.Decrypt(note.Content, keyBytes);
            }

            throw new Exception();
        }
    }
}
