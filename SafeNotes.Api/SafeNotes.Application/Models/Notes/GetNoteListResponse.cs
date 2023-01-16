using static SafeNotes.Application.Models.Notes.GetNoteListResponse;

namespace SafeNotes.Application.Models.Notes
{
    public record GetNoteListResponse(IEnumerable<NotePreview> UserNotes, IEnumerable<NotePreview> SharedToUserNotes)
    {
        public record NotePreview(int Id, string Title, bool IsPublic, bool? IsEncryptedWithUserSpecifiedKey, DateTime LastModified, bool CanWrite, bool IsOwner);
    }
}
