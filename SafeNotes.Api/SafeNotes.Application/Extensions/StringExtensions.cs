namespace SafeNotes.Application.Extensions
{
    internal static class StringExtensions
    {
        private const string DangerousCharacters = "`'\"\0";

        public static bool ContainsSpecialCharacter(this string value)
            => value.Any(x => !char.IsLetterOrDigit(x));

        public static bool ContainsDigitsCharacter(this string value)
            => value.Any(x => char.IsDigit(x));

        public static bool ContainsLetterCharacter(this string value)
            => value.Any(x => char.IsLetter(x));

        public static bool DoesNotContainDangerousCharacters(this string value)
            => !value.Any(x => DangerousCharacters.Contains(x));
    }
}
