using SafeNotes.Domain.Entities;

namespace SafeNotes.Application.Authorization
{
    public interface IUserIdentityProvider
    {
        Task<User> GetUser(CancellationToken cancellationToken);
    }
}
