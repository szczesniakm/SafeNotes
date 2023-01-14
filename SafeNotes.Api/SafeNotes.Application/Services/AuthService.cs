using FluentValidation;
using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Logging;
using SafeNotes.Application.Models;
using SafeNotes.Application.Models.Auth;
using SafeNotes.Application.Models.Users;
using SafeNotes.Application.Utils;
using SafeNotes.Domain.Repositories;

namespace SafeNotes.Application.Services
{
    public class AuthService
    {
        private readonly IUsersRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IValidator<LoginRequest> _loginRequestValidator;
        private readonly JwtTokenGenerator _jwtGenerator;

        public AuthService(
            IUsersRepository userRepository,
            IValidator<LoginRequest> loginRequestValidator,
            ILogger<AuthService> logger,
            JwtTokenGenerator jwtGenerator)
        {
            _userRepository = userRepository;
            _loginRequestValidator = loginRequestValidator;
            _logger = logger;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<Result<JwtTokenResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _loginRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByEmail(request.Email, cancellationToken);

            if (user == null)
            {
                return new Exception();
            }

            if (!user.IsEmailConfirmed)
            {
                return new Exception();
            }

            if(!Argon2.Verify(user.PasswordHash, request.Password))
            {
                return new Exception();
            }

            return new JwtTokenResponse(_jwtGenerator.GenerateToken(user));
        }
    }
}
