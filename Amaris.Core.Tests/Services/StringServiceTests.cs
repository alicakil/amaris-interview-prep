using Amaris.Core.Services;

namespace Amaris.Core.Tests.Services;

public class StringServiceTests
{
    private readonly IStringService _sut = new StringService();

    [Fact]
    public void Reverse_ValidString_ReturnsReversed()
    {
        var result = _sut.Reverse("hello");

        Assert.Equal("olleh", result);
    }

    [Fact]
    public void Reverse_EmptyString_ReturnsEmpty()
    {
        var result = _sut.Reverse(string.Empty);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Reverse_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.Reverse(null!));
    }

    [Theory]
    [InlineData("racecar", true)]
    [InlineData("A man a plan a canal Panama", true)]
    [InlineData("hello", false)]
    public void IsPalindrome_VariousInputs_ReturnsExpected(string input, bool expected)
    {
        var result = _sut.IsPalindrome(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("hello world", 2)]
    [InlineData("one", 1)]
    [InlineData("  spaced   out  ", 2)]
    [InlineData("", 0)]
    [InlineData("   ", 0)]
    public void WordCount_VariousInputs_ReturnsExpectedCount(string input, int expected)
    {
        var result = _sut.WordCount(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("hello", "Hello")]
    [InlineData("world", "World")]
    [InlineData("", "")]
    public void Capitalize_VariousInputs_ReturnsCapitalized(string input, string expected)
    {
        var result = _sut.Capitalize(input);

        Assert.Equal(expected, result);
    }
}
