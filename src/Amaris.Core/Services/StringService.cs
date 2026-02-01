namespace Amaris.Core.Services;

public class StringService : IStringService
{
    public string Reverse(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var chars = input.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    public bool IsPalindrome(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var cleaned = input.Replace(" ", "").ToLowerInvariant();
        var reversed = Reverse(cleaned);
        return cleaned == reversed;
    }

    public int WordCount(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return 0;

        return input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public string Capitalize(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return string.Concat(input[0].ToString().ToUpperInvariant(), input.AsSpan(1));
    }
}
