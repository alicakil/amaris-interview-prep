namespace Amaris.Core.Services;

public interface IStringService
{
    string Reverse(string input);
    bool IsPalindrome(string input);
    int WordCount(string input);
    string Capitalize(string input);
}
