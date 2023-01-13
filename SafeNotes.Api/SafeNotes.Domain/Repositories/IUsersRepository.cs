using SafeNotes.Domain.Entities;

namespace SafeNotes.Domain.Repositories
{
    public interface IUsersRepository
    {
        Task AddUser(User user, CancellationToken cancellationToken);

        Task<User?> GetByEmail(string email, CancellationToken cancellationToken);

        Task<User?> GetBySecurityCode(string securityCode, CancellationToken cancellationToken);

        Task UpdateUser(User user, CancellationToken cancellationToken);

        Task DeleteUser(User user, CancellationToken cancellationToken);
    }
}
