namespace SafeNotes.Infrastructure.Emails
{
    public class EmailMessage
    {
        public string Email { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public EmailMessage(string email, string subject, string body) 
        { 
            Email = email;
            Subject = subject;
            Body = body;
        }
    }
}
