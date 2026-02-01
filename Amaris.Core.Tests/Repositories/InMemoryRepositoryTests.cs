using Amaris.Core.Models;
using Amaris.Core.Repositories;
using Amaris.Data.Repositories;
using FluentAssertions;

namespace Amaris.Core.Tests.Repositories;

public class InMemoryRepositoryTests
{
    private readonly IRepository<Product> _sut = new InMemoryRepository<Product>();

    private static Product CreateProduct(int id = 1, string name = "Test", decimal price = 9.99m)
        => new() { Id = id, Name = name, Price = price, Category = "General" };

    [Fact]
    public void Add_NewEntity_CanBeRetrievedById()
    {
        var product = CreateProduct();

        _sut.Add(product);

        var result = _sut.GetById(1);
        result.Should().NotBeNull()
            .And.Subject.As<Product>().Name.Should().Be("Test");
    }

    [Fact]
    public void Add_DuplicateId_ThrowsInvalidOperationException()
    {
        _sut.Add(CreateProduct());

        var act = () => _sut.Add(CreateProduct());

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetById_NonExistent_ReturnsNull()
    {
        var result = _sut.GetById(999);

        result.Should().BeNull();
    }

    [Fact]
    public void GetAll_MultipleEntities_ReturnsAll()
    {
        _sut.Add(CreateProduct(1, "A"));
        _sut.Add(CreateProduct(2, "B"));
        _sut.Add(CreateProduct(3, "C"));

        var result = _sut.GetAll().ToList();

        result.Should().HaveCount(3);
    }

    [Fact]
    public void Update_ExistingEntity_ReflectsChanges()
    {
        _sut.Add(CreateProduct(1, "Old"));

        _sut.Update(CreateProduct(1, "New"));

        var result = _sut.GetById(1);
        result.Should().NotBeNull()
            .And.Subject.As<Product>().Name.Should().Be("New");
    }

    [Fact]
    public void Update_NonExistent_ThrowsKeyNotFoundException()
    {
        var act = () => _sut.Update(CreateProduct(999));

        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void Delete_ExistingEntity_RemovesIt()
    {
        _sut.Add(CreateProduct());

        _sut.Delete(1);

        _sut.GetById(1).Should().BeNull();
    }

    [Fact]
    public void Delete_NonExistent_ThrowsKeyNotFoundException()
    {
        var act = () => _sut.Delete(999);

        act.Should().Throw<KeyNotFoundException>();
    }
}
