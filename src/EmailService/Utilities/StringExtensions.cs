namespace GaEpd.EmailService.Utilities;

internal static class StringExtensions
{
    public static string ConcatWithSeparator(this IEnumerable<string?> items, string separator = " ") =>
        string.Join(separator, items.Where(s => !string.IsNullOrEmpty(s)));
}
