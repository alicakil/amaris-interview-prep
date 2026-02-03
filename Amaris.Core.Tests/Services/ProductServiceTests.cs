using Amaris.Core.Models;
using Amaris.Core.Repositories;
using Amaris.Core.Services;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;

namespace Amaris.Core.Tests.Services;

/// <summary>
/// Tests for <see cref="ProductService"/> demonstrating:
/// - Moq for mocking dependencies (Setup, Verify, It.IsAny)
/// - FluentAssertions for readable assertions (Should(), Be(), HaveCount())
/// - AutoFixture for automatic test data generation ([AutoData])
/// - xUnit best practices: Arrange-Act-Assert, Theory/MemberData
/// </summary>
public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly ProductService _sut;
    private readonly Fixture _fixture;

    /// <summary>
    /// Constructor runs before each test (xUnit creates a new instance per test).
    /// This replaces [SetUp] from NUnit.
    /// </summary>
    public ProductServiceTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _sut = new ProductService(_repoMock.Object);
        _fixture = new Fixture();
    }

    #region GetById - Moq Setup + FluentAssertions

    /// <summary>
    /// Moq Setup: configures GetById(1) to return a specific product.
    /// FluentAssertions: Should().NotBeNull() and .Be() for clean assertions.
    /// </summary>
    [Fact]
    public void GetById_ExistingId_ReturnsProduct()
    {
        // Arrange
        var expected = new Product { Id = 1, Name = "Laptop", Price = 999m, Category = "Electronics" };
        _repoMock.Setup(r => r.GetById(1)).Returns(expected);

        // Act
        var result = _sut.GetById(1);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Laptop");
        result.Price.Should().Be(999m);
    }

    /// <summary>
    /// Moq It.IsAny: matches any integer argument.
    /// </summary>
    [Fact]
    public void GetById_NonExistingId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((Product?)null);

        var result = _sut.GetById(999);

        result.Should().BeNull();
    }

    #endregion

    #region GetByCategory - Collection assertions

    /// <summary>
    /// FluentAssertions collection: HaveCount() and AllSatisfy() for collections.
    /// </summary>
    [Fact]
    public void GetByCategory_MatchingProducts_ReturnsFiltered()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Laptop", Price = 999m, Category = "Electronics" },
            new() { Id = 2, Name = "Shirt", Price = 29m, Category = "Clothing" },
            new() { Id = 3, Name = "Phone", Price = 699m, Category = "Electronics" }
        };
        _repoMock.Setup(r => r.GetAll()).Returns(products);

        // Act
        var result = _sut.GetByCategory("Electronics").ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(p => p.Category.Should().Be("Electronics"));
    }

    [Fact]
    public void GetByCategory_NoMatch_ReturnsEmpty()
    {
        _repoMock.Setup(r => r.GetAll()).Returns(new List<Product>());

        var result = _sut.GetByCategory("NonExistent");

        result.Should().BeEmpty();
    }

    /// <summary>
    /// FluentAssertions exception: Throw + WithParameterName for precise checks.
    /// </summary>
    [Fact]
    public void GetByCategory_NullCategory_ThrowsArgumentNullException()
    {
        var act = () => _sut.GetByCategory(null!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("category");
    }

    #endregion

    #region GetExpensive - Ordering assertions

    /// <summary>
    /// FluentAssertions ordering: BeInDescendingOrder() verifies sort order.
    /// NotContain() verifies exclusion.
    /// </summary>
    [Fact]
    public void GetExpensive_ReturnsOnlyExpensiveProducts_OrderedByPriceDesc()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Mouse", Price = 25m, Category = "Electronics" },
            new() { Id = 2, Name = "Laptop", Price = 1500m, Category = "Electronics" },
            new() { Id = 3, Name = "Monitor", Price = 500m, Category = "Electronics" }
        };
        _repoMock.Setup(r => r.GetAll()).Returns(products);

        var result = _sut.GetExpensive(100m).ToList();

        result.Should().HaveCount(2);
        result.Should().BeInDescendingOrder(p => p.Price);
        result.Should().NotContain(p => p.Name == "Mouse");
    }

    #endregion

    #region Create - Moq Verify (interaction testing)

    /// <summary>
    /// Moq Verify: ensures Add was called exactly once with the correct argument.
    /// This is interaction testing - verifying the SUT talks to its dependency correctly.
    /// </summary>
    [Fact]
    public void Create_ValidProduct_CallsRepositoryAdd()
    {
        var product = new Product { Id = 1, Name = "Keyboard", Price = 75m, Category = "Electronics" };

        _sut.Create(product);

        _repoMock.Verify(r => r.Add(product), Times.Once);
    }

    /// <summary>
    /// Moq Verify Times.Never: ensures Add was NOT called when validation fails.
    /// </summary>
    [Fact]
    public void Create_NullProduct_ThrowsAndDoesNotCallRepository()
    {
        var act = () => _sut.Create(null!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("product");

        _repoMock.Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
    }

    /// <summary>
    /// FluentAssertions WithMessage: wildcard pattern matching on exception messages.
    /// </summary>
    [Fact]
    public void Create_EmptyName_ThrowsArgumentException()
    {
        var product = new Product { Id = 1, Name = "", Price = 10m, Category = "Test" };

        var act = () => _sut.Create(product);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*name*required*");

        _repoMock.Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
    }

    #endregion

    #region AutoFixture - Automatic test data generation

    /// <summary>
    /// AutoFixture [AutoData]: automatically generates random test data.
    /// No need to manually create Product - AutoFixture fills all properties.
    /// Useful when you don't care about specific values, just that the flow works.
    /// </summary>
    [Theory, AutoData]
    public void Create_AutoGeneratedProduct_CallsRepositoryAdd(Product product)
    {
        // AutoFixture generates a Product with random Id, Name, Price, Category.
        // Name will be a random non-empty string, so validation passes.
        _sut.Create(product);

        _repoMock.Verify(r => r.Add(product), Times.Once);
    }

    /// <summary>
    /// AutoFixture Fixture.Create: generate test data inline when you need control.
    /// Combine with Moq Setup for realistic scenarios.
    /// </summary>
    [Fact]
    public void GetByCategory_WithAutoFixtureData_ReturnsCorrectProducts()
    {
        // Fixture.CreateMany generates 3 random products by default
        var products = _fixture.CreateMany<Product>(3).ToList();
        // Override category on first product so we have a known match
        products[0].Category = "TargetCategory";

        _repoMock.Setup(r => r.GetAll()).Returns(products);

        var result = _sut.GetByCategory("TargetCategory").ToList();

        result.Should().ContainSingle();
        result[0].Should().BeSameAs(products[0]);
    }

    /// <summary>
    /// AutoFixture Build: customize object creation with .With() and .Without().
    /// Useful when you need partial control over generated data.
    /// </summary>
    [Fact]
    public void GetExpensive_WithCustomFixtureData_FiltersCorrectly()
    {
        var cheapProduct = _fixture.Build<Product>()
            .With(p => p.Price, 10m)       // Force specific price
            .Create();

        var expensiveProduct = _fixture.Build<Product>()
            .With(p => p.Price, 500m)
            .Create();

        _repoMock.Setup(r => r.GetAll()).Returns(new[] { cheapProduct, expensiveProduct });

        var result = _sut.GetExpensive(100m).ToList();

        result.Should().ContainSingle()
            .Which.Price.Should().Be(500m);
    }

    #endregion

    #region Theory + MemberData - Parameterized tests with complex data

    /// <summary>
    /// MemberData: references a static property for complex test data.
    /// Better than InlineData when data needs constructor logic or complex types.
    /// TheoryData{T1,T2} provides strong typing.
    /// </summary>
    [Theory]
    [MemberData(nameof(ExpensiveThresholdTestData))]
    public void GetExpensive_VariousThresholds_ReturnsExpectedCount(
        decimal threshold, int expectedCount)
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "A", Price = 50m, Category = "X" },
            new() { Id = 2, Name = "B", Price = 150m, Category = "X" },
            new() { Id = 3, Name = "C", Price = 500m, Category = "X" },
            new() { Id = 4, Name = "D", Price = 1000m, Category = "X" }
        };
        _repoMock.Setup(r => r.GetAll()).Returns(products);

        var result = _sut.GetExpensive(threshold);

        result.Should().HaveCount(expectedCount);
    }

    public static TheoryData<decimal, int> ExpensiveThresholdTestData => new()
    {
        { 0m, 4 },     // All products are > 0
        { 100m, 3 },   // 150, 500, 1000
        { 500m, 1 },   // Only 1000
        { 9999m, 0 }   // None
    };

    #endregion
}
