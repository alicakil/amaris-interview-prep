using Amaris.Core.Models;
using Amaris.Core.Repositories;

namespace Amaris.Data.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<int, Product> _store = new();

    public Product? GetById(int id)
    {
        _store.TryGetValue(id, out var product);
        return product;
    }

    public IEnumerable<Product> GetAll() => _store.Values;

    public void Add(Product product)
    {
        if (_store.ContainsKey(product.Id))
            throw new InvalidOperationException($"Product with Id {product.Id} already exists.");

        _store[product.Id] = product;
    }

    public void Update(Product product)
    {
        if (!_store.ContainsKey(product.Id))
            throw new KeyNotFoundException($"Product with Id {product.Id} not found.");

        _store[product.Id] = product;
    }

    public void Delete(int id)
    {
        if (!_store.Remove(id))
            throw new KeyNotFoundException($"Product with Id {id} not found.");
    }
}
