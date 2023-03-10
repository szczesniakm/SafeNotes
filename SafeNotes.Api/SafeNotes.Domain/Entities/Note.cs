namespace SafeNotes.Domain.Entities
{
    public class Note
    {
        public int Id { get; private set; }

        public string OwnerEmail { get; private set; }

        public User Owner { get; private set; }

        public string Title { get; private set; }

        public string Content { get; private set; }

        public bool IsPublic { get; private set; }

        public bool IsEncryptedWithUserSpecifiedKey { get; private set; }

        public string? Salt { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime LastModifiedAt { get; private set; }

        public string LastModifiedBy { get; private set; }

        public List<UserAccess> AllowedUsers { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Note()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public Note(User owner, string title, string content, bool isPublic, bool isEncryptedWithUserSpecifiedKey, string? salt)
        {
            Owner = owner;
            Title = title;
            Content = content;
            IsPublic = isPublic;
            IsEncryptedWithUserSpecifiedKey = isEncryptedWithUserSpecifiedKey;
            Salt = salt;
            CreatedAt = DateTime.UtcNow;
            LastModifiedAt = DateTime.UtcNow;
            LastModifiedBy = owner.Email;
        }

        public void UpdateAllowedUsers(List<UserAccess> allowedUsers)
        {
            AllowedUsers = allowedUsers;
        }

        public void Update(string title, string content, string modifiedBy)
        {
            Title = title;
            Content = content;
            LastModifiedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }
    }
}
