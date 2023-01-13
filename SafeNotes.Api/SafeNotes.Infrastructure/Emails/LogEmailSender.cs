using Microsoft.Extensions.Logging;

namespace SafeNotes.Infrastructure.Emails
{
    internal class LogEmailSender : IEmailSender
    {
        private readonly ILogger<LogEmailSender> _logger;

        public LogEmailSender(ILogger<LogEmailSender> logger)
        {
            _logger = logger;
        }

        public async Task SendEmail(EmailMessage message)
        {
            _logger.Log(LogLevel.Debug, $"Email: {message.Email}", LogLevel.Debug);
            _logger.Log(LogLevel.Debug, $"Subject: {message.Subject}", LogLevel.Debug);
            _logger.Log(LogLevel.Debug, $"Message: {message.Body}", LogLevel.Debug);

            await Task.CompletedTask;
        }
    }
}
