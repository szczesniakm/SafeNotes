namespace SafeNotes.Domain.Entities;

public class User
{
    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public bool IsEmailConfirmed { get; private set; }

    public string SecurityCode { get; private set; }

    public DateTime SecurityCodeExpirationDate { get; private set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public User(string email, string passwordHash, string securityCode, DateTime securityCodeExpirationDate)
    {
        Email = email;
        PasswordHash = passwordHash;
        IsEmailConfirmed = false;
        SecurityCode = securityCode;
        SecurityCodeExpirationDate = securityCodeExpirationDate;
    }

    public void ConfirmEmail()
    {
        IsEmailConfirmed = true;
        SecurityCode = string.Empty;
        SecurityCodeExpirationDate = DateTime.MinValue;
    }
}
