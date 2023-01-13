namespace SafeNotes.Infrastructure.Emails
{
    public interface IEmailSender
    {
        Task SendEmail(EmailMessage message);
    }
}
