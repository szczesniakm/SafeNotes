namespace SafeNotes.Application.Models.Notes
{
    public record GetNoteResponse(string Title,
        string Content,
        bool IsPublic,
        bool IsEncryptedWithUserSpecifiedKey,
        DateTime LastModifiedOn);
}
