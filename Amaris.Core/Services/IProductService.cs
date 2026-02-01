namespace Amaris.Core.Services;

using Amaris.Core.Models;

public interface IProductService
{
    Product? GetById(int id);
    IEnumerable<Product> GetByCategory(string category);
    IEnumerable<Product> GetExpensive(decimal threshold);
    void Create(Product product);
}
