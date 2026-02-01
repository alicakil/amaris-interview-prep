namespace Amaris.Core.Services;

public class CalculatorService : ICalculatorService
{
    public double Add(double a, double b) => a + b;

    public double Subtract(double a, double b) => a - b;

    public double Multiply(double a, double b) => a * b;

    public double Divide(double a, double b)
    {
        if (b == 0)
            throw new DivideByZeroException("Cannot divide by zero.");

        return a / b;
    }

    public long Factorial(int n)
    {
        if (n < 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Factorial is not defined for negative numbers.");

        long result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;

        return result;
    }
}
