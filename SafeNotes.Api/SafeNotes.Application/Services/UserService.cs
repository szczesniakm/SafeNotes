using FluentValidation;
using Isopoh.Cryptography.Argon2;
using SafeNotes.Application.Models;
using SafeNotes.Application.Models.Users;
using SafeNotes.Application.Utils;
using SafeNotes.Domain.Entities;
using SafeNotes.Domain.Repositories;
using SafeNotes.Infrastructure.Emails;

namespace SafeNotes.Application.Services
{
    public class UserService
    {
        private readonly IUsersRepository _userRepository;
        private readonly IValidator<RegisterUserRequest> _registerUserRequestValidator;
        private readonly IEmailSender _emailSender;

        public UserService(
            IUsersRepository userRepository,
            IValidator<RegisterUserRequest> registerUserRequestValidator,
            IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _registerUserRequestValidator = registerUserRequestValidator;
            _emailSender = emailSender;
        }

        public async Task<Result> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _registerUserRequestValidator.ValidateAsync(request, cancellationToken);
            if(!validationResult.IsValid)
            {
                return new ValidationException(validationResult.Errors);
            }

            var hashedPassword = Argon2.Hash(request.Password);
            var emailConfirmationCode = RandomSecurityStringGenerator.Generate(64);
            var emailConfirmationCodeExpiration = DateTime.UtcNow.AddDays(1);

            var user = new User(request.Email, hashedPassword, emailConfirmationCode, emailConfirmationCodeExpiration);

            await _userRepository.AddUser(user, cancellationToken);

            await _emailSender.SendEmail(
                new EmailMessage(
                    user.Email,
                    "Verify your email",
                    $"Click in the link below to activate your account on safenotes.com \n Verification code: {emailConfirmationCode}"));

            return Result.Ok;
        }

        public async Task<Result> ConfirmEmail(string emailConfirmationCode, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetBySecurityCode(emailConfirmationCode, cancellationToken);
            if (user is null)
            {
                return new Exception();
            }

            if (user.SecurityCodeExpirationDate < DateTime.UtcNow)
            {
                return new Exception();
            }

            user.ConfirmEmail();

            await _userRepository.UpdateUser(user, cancellationToken);

            return Result.Ok;
        }
    }
}
