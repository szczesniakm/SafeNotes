namespace SafeNotes.Application.Authorization
{
    public interface IUserAccessProvider
    {
        Task<bool> CanRead(int noteId, CancellationToken cancellationToken);

        Task<bool> CanWrite(int noteId, CancellationToken cancellationToken);
    }
}
