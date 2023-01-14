using Microsoft.AspNetCore.Http;
using SafeNotes.Domain.Entities;
using SafeNotes.Domain.Repositories;
using System.Security.Claims;

namespace SafeNotes.Application.Authorization
{
    internal class UserIdentityProvider : IUserIdentityProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersRepository _userRepository;

        public UserIdentityProvider(IHttpContextAccessor httpContext, IUsersRepository userRepository)
        {
            _httpContextAccessor = httpContext;
            _userRepository = userRepository;
        }

        public async Task<User> GetUser(CancellationToken cancellationToken)
        {
            var claims = _httpContextAccessor.HttpContext.User.Identities.First().Claims.ToList();

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email == null)
            {
                throw new InvalidOperationException("Invalid user claims.");
            }

            var user = await _userRepository.GetByEmail(email, cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException("Could not retrieve user data. User does not exits.");
            }

            return user;
        }
    }
}
