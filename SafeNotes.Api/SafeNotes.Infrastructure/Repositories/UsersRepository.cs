using Microsoft.EntityFrameworkCore;
using SafeNotes.Domain.Entities;
using SafeNotes.Domain.Repositories;

namespace SafeNotes.Infrastructure.Repositories
{
    internal class UsersRepository : IUsersRepository
    {
        private readonly SafeNotesContext _context;

        public UsersRepository(SafeNotesContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user, CancellationToken cancellationToken)
        {
            _context.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUser(User user, CancellationToken cancellationToken)
        {
            _context.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
            => await _context.Set<User>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        public async Task<User?> GetBySecurityCode(string securityCode, CancellationToken cancellationToken)
            => await _context.Set<User>().FirstOrDefaultAsync(x => x.SecurityCode == securityCode, cancellationToken);

        public async Task UpdateUser(User user, CancellationToken cancellationToken)
        {
            _context.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
