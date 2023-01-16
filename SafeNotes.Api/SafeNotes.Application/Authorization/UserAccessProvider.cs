using SafeNotes.Domain.Repositories;

namespace SafeNotes.Application.Authorization
{
    internal class UserAccessProvider : IUserAccessProvider
    {
        private INotesRepository _notesRepository;
        private IUserIdentityProvider _userIdentityProvider;

        public UserAccessProvider(IUserIdentityProvider userIdentityProvider,
            INotesRepository notesRepository)
        {
            _userIdentityProvider = userIdentityProvider;
            _notesRepository = notesRepository;
        }

        public async Task<bool> CanRead(int noteId, CancellationToken cancellationToken)
        {
            var note = await _notesRepository.GetById(noteId, CancellationToken.None);
            var user = await _userIdentityProvider.GetUser(CancellationToken.None);
            if (note is null)
            {
                throw new InvalidOperationException("Could not find note in database.");
            }

            if (note.Owner == user)
            {
                return true;
            }

            if (note.AllowedUsers is not null && note.AllowedUsers.Any(x => x.UserEmail == user.Email && x.CanRead))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> CanWrite(int noteId, CancellationToken cancellationToken)
        {
            var note = await _notesRepository.GetById(noteId, CancellationToken.None);
            var user = await _userIdentityProvider.GetUser(CancellationToken.None);
            if (note is null)
            {
                throw new InvalidOperationException("Could not find note in database.");
            }

            if (note.Owner == user)
            {
                return true;
            }

            if (note.AllowedUsers is not null && note.AllowedUsers.Any(x => x.UserEmail == user.Email && x.CanWrite))
            {
                return true;
            }

            return false;
        }
    }
}
