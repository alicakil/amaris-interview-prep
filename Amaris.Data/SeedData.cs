using Amaris.Core.Models;
using Amaris.Core.Repositories;

namespace Amaris.Data;

public static class SeedData
{
    public static void Initialize(IProductRepository repository)
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Laptop", Price = 1299.99m, Category = "Electronics" },
            new() { Id = 2, Name = "Mouse", Price = 25.50m, Category = "Electronics" },
            new() { Id = 3, Name = "Desk Chair", Price = 349.00m, Category = "Furniture" },
            new() { Id = 4, Name = "Notebook", Price = 4.99m, Category = "Stationery" },
            new() { Id = 5, Name = "Monitor", Price = 599.00m, Category = "Electronics" }
        };

        foreach (var product in products)
        {
            repository.Add(product);
        }
    }
}
