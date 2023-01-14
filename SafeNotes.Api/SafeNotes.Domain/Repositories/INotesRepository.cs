using SafeNotes.Domain.Entities;

namespace SafeNotes.Domain.Repositories
{
    public interface INotesRepository
    {
        Task Add(Note note, CancellationToken cancellationToken);

        Task<bool> Exists(int id, CancellationToken cancellationToken);

        Task<Note?> GetById(int id, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetByUserEmail(string email, CancellationToken cancellationToken);

        Task<IEnumerable<Note>> GetSharedByUserEmail(string email, CancellationToken cancellationToken);

        Task Update(Note note, CancellationToken cancellationToken);

        Task Delete(Note note, CancellationToken cancellationToken);
    }
}
