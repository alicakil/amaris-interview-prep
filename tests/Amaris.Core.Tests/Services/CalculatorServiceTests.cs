using Amaris.Core.Services;

namespace Amaris.Core.Tests.Services;

public class CalculatorServiceTests
{
    private readonly ICalculatorService _sut = new CalculatorService();

    [Fact]
    public void Add_TwoPositiveNumbers_ReturnsSum()
    {
        var result = _sut.Add(3, 5);

        Assert.Equal(8, result);
    }

    [Theory]
    [InlineData(10, 3, 7)]
    [InlineData(0, 0, 0)]
    [InlineData(-5, -3, -2)]
    public void Subtract_VariousInputs_ReturnsExpectedResult(double a, double b, double expected)
    {
        var result = _sut.Subtract(a, b);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(3, 4, 12)]
    [InlineData(0, 100, 0)]
    [InlineData(-2, 5, -10)]
    public void Multiply_VariousInputs_ReturnsExpectedResult(double a, double b, double expected)
    {
        var result = _sut.Multiply(a, b);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Divide_ValidInputs_ReturnsQuotient()
    {
        var result = _sut.Divide(10, 2);

        Assert.Equal(5, result);
    }

    [Fact]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        Assert.Throws<DivideByZeroException>(() => _sut.Divide(10, 0));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(5, 120)]
    [InlineData(10, 3628800)]
    public void Factorial_ValidInputs_ReturnsExpectedResult(int input, long expected)
    {
        var result = _sut.Factorial(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Factorial_NegativeNumber_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _sut.Factorial(-1));
    }
}
