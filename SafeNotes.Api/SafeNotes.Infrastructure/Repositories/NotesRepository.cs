using Microsoft.EntityFrameworkCore;
using SafeNotes.Domain.Entities;
using SafeNotes.Domain.Repositories;

namespace SafeNotes.Infrastructure.Repositories
{
    internal class NotesRepository : INotesRepository
    {
        private readonly SafeNotesContext _context;

        public NotesRepository(SafeNotesContext context)
        {
            _context = context;
        }

        public async Task Add(Note note, CancellationToken cancellationToken)
        {
            _context.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Note note, CancellationToken cancellationToken)
        {
            _context.Remove(note);
            await _context.SaveChangesAsync();
        }

        public async Task<Note?> GetById(int id, CancellationToken cancellationToken)
            => await _context.Set<Note>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<IEnumerable<Note>> GetByUserEmail(string email, CancellationToken cancellationToken)
            => await _context.Set<Note>().Include(x => x.Owner).Include(x => x.AllowedUsers).Where(x => x.Owner.Email == email).ToListAsync(cancellationToken);

        public async Task<IEnumerable<Note>> GetSharedByUserEmail(string email, CancellationToken cancellationToken)
            => await _context.Set<Note>().Include(x => x.Owner).Include(x => x.AllowedUsers).Where(x => x.AllowedUsers.Any(x => x.UserEmail == email)).ToListAsync(cancellationToken);

        public async Task Update(Note note, CancellationToken cancellationToken)
        {
            _context.Update(note);
            await _context.SaveChangesAsync();
        }
    }
}
