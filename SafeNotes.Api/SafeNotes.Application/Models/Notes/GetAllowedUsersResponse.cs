namespace SafeNotes.Application.Models.Notes
{
    public record GetAllowedUsersResponse(IEnumerable<AllowedUser> AllowedUsers);
}
