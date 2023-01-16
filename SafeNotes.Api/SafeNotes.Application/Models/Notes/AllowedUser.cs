namespace SafeNotes.Application.Models.Notes
{
    public record AllowedUser(string? Email, bool? CanRead, bool? CanWrite);
}
