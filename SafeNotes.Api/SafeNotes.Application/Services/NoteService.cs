using FluentValidation;
using Ganss.Xss;
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
        private readonly IValidator<UpdateNoteModel> _updateNoteModelValidator;
        private readonly IValidator<UpdateAllowedUsersRequest> _updateAllowedUsersRequestValidator;
        private readonly IValidator<GetNoteModel> _getNoteModelValidator;
        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly IUserAccessProvider _userAccessProvider;

        public NoteService(
            INotesRepository notesRepository,
            IValidator<CreateNoteRequest> createNoteRequestValidator,
            IValidator<UpdateNoteModel> updateNoteModelValidator,
            IValidator<UpdateAllowedUsersRequest> updateAllowedUsersRequestValidator,
            IValidator<GetNoteModel> getNoteModelValidator,
            IUserIdentityProvider userIdentityProvider,
            IUserAccessProvider userAccessProvider)
        {
            _notesRepository = notesRepository;
            _createNoteRequestValidator = createNoteRequestValidator;
            _updateNoteModelValidator = updateNoteModelValidator;
            _updateAllowedUsersRequestValidator = updateAllowedUsersRequestValidator;
            _getNoteModelValidator = getNoteModelValidator;
            _userIdentityProvider = userIdentityProvider;
            _userAccessProvider = userAccessProvider;
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

        public async Task<Result> UpdateNote(UpdateNoteModel request, CancellationToken cancellationToken)
        {
            var validationResult = await _updateNoteModelValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ValidationException(validationResult.Errors);
            }

            var canWrite = await _userAccessProvider.CanWrite(request.NoteId, cancellationToken);
            if (!canWrite)
            {
                return new Exception("Forbidden");
            }

            var user = await _userIdentityProvider.GetUser(cancellationToken);

            var note = await _notesRepository.GetById(request.NoteId, cancellationToken);

            var sanitizer = new HtmlSanitizer();
            var content = sanitizer.Sanitize(request.Content!);

            if(note!.IsEncryptedWithUserSpecifiedKey)
            {
                if(request.Key is null)
                {
                    return new Exception();
                }
                var hashedKey = EncryptionHelper.HashKey(request.Key, note.Salt!);
                EncryptionHelper.Decrypt(content, hashedKey);
                content = EncryptionHelper.Encrypt(content, hashedKey);
            }

            note.Update(request.Title, content, user.Email);

            await _notesRepository.Update(note, cancellationToken);

            return Result.Ok;
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

        public async Task<Result<GetAllowedUsersResponse>> GetAllowedUsers(int noteId, CancellationToken cancellationToken)
        {
            var user = await _userIdentityProvider.GetUser(cancellationToken);

            var note = await _notesRepository.GetById(noteId, cancellationToken);

            if (note!.OwnerEmail != user.Email)
            {
                return new Exception("Not Found");
            }

            return new GetAllowedUsersResponse(
                note.AllowedUsers.Select(x => new AllowedUser(
                    x.UserEmail,
                    x.CanRead,
                    x.CanWrite)).ToList());
        }

        public async Task<Result<GetNoteListResponse>> GetNoteList(CancellationToken cancellationToken)
        {
            var user = await _userIdentityProvider.GetUser(cancellationToken);

            var userNotes = await _notesRepository.GetByUserEmail(user.Email, cancellationToken);

            var sharedToUserNotes = await _notesRepository.GetSharedByUserEmail(user.Email, cancellationToken);

            return new GetNoteListResponse(
                userNotes.Select(x => new GetNoteListResponse.NotePreview(x.Id, x.Title, x.IsPublic, x.IsEncryptedWithUserSpecifiedKey, x.LastModifiedAt, true, true)).ToList(),
                sharedToUserNotes
                    .Select(x => new GetNoteListResponse.NotePreview(
                        x.Id,
                        x.Title,
                        x.IsPublic,
                        x.IsEncryptedWithUserSpecifiedKey,
                        x.LastModifiedAt,
                        x.AllowedUsers.Where(allowed => allowed.UserEmail == user.Email).Select(x => x.CanWrite).FirstOrDefault(),
                        false))
                    .ToList());
        }

        public async Task<Result<GetNoteResponse>> GetNote(GetNoteModel request, CancellationToken cancellationToken)
        {
            var validationResult = await _getNoteModelValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ValidationException(validationResult.Errors);
            }

            var canRead = await _userAccessProvider.CanRead(request.NoteId, cancellationToken);
            if(!canRead)
            {
                return new Exception("Unauthorized");
            }

            var note = await _notesRepository.GetById(request.NoteId, cancellationToken);
            if (note is null)
            {
                return new Exception("Note not found");
            }

            if(note.IsEncryptedWithUserSpecifiedKey)
            {
                if(request.Key is null)
                {
                    return new Exception("Key is null");
                }
                var decryptedContent = DecryptNote(note, request.Key!);
                return new GetNoteResponse(note.Title, decryptedContent, note.IsPublic, note.IsEncryptedWithUserSpecifiedKey, note.LastModifiedAt);
            }

            return new GetNoteResponse(note.Title, note.Content, note.IsPublic, note.IsEncryptedWithUserSpecifiedKey, note.LastModifiedAt);
        }

        public string DecryptNote(Note note, string key)
        {
            if (!note.IsPublic && note.IsEncryptedWithUserSpecifiedKey)
            {
                var keyBytes = EncryptionHelper.HashKey(key, note.Salt!);
                try
                {
                    return EncryptionHelper.Decrypt(note.Content, keyBytes);
                } catch(Exception ex)
                {
                    throw new Exception("Invalid key.");
                }
            }

            throw new Exception();
        }
    }
}
