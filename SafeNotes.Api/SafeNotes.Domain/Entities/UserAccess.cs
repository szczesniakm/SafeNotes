namespace SafeNotes.Domain.Entities
{
    public class UserAccess
    {
        public int NoteId { get; private set; }

        public Note Note { get; private set; }

        public string UserEmail { get; private set; }

        public bool CanRead { get; private set; }

        public bool CanWrite { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public UserAccess()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public UserAccess(string userEmail, bool canRead, bool canWrite)
        {
            UserEmail = userEmail;
            CanRead = canRead;
            CanWrite = canWrite;
        }
    }
}