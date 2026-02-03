using Amaris.Core.Models;
using Amaris.Core.Repositories;

namespace Amaris.Core.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    private readonly IProductRepository _repository = repository;

    public Product? GetById(int id) => _repository.GetById(id);

    public IEnumerable<Product> GetByCategory(string category)
    {
        ArgumentNullException.ThrowIfNull(category);

        return _repository.GetAll()
            .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Product> GetExpensive(decimal threshold)
    {
        return _repository.GetAll()
            .Where(p => p.IsExpensive(threshold))
            .OrderByDescending(p => p.Price);
    }

    public void Create(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required.", nameof(product));

        _repository.Add(product);
    }
}
