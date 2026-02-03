using Amaris.Api.DTOs;
using Amaris.Core.Models;
using Amaris.Core.Repositories;

namespace Amaris.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var products = app.MapGroup("/api/products")
            .WithTags("Products");

        products.MapGet("/", (IProductRepository repo) =>
            Results.Ok(repo.GetAll().Select(ToResponse)))
            .WithName("GetAllProducts")
            .WithSummary("Get all products")
            .WithDescription("Returns a list of all products in the store.");

        products.MapGet("/{id:int}", (int id, IProductRepository repo) =>
            repo.GetById(id) is { } product
                ? Results.Ok(ToResponse(product))
                : Results.NotFound())
            .WithName("GetProductById")
            .WithSummary("Get a product by ID")
            .WithDescription("Returns a single product matching the given ID, or 404 if not found.");

        products.MapPost("/", (CreateProductRequest request, IProductRepository repo) =>
        {
            var product = new Product
            {
                Id = repo.GetAll().Any() ? repo.GetAll().Max(p => p.Id) + 1 : 1,
                Name = request.Name,
                Price = request.Price,
                Category = request.Category
            };
            repo.Add(product);
            return Results.Created($"/api/products/{product.Id}", ToResponse(product));
        })
            .WithName("CreateProduct")
            .WithSummary("Create a new product")
            .WithDescription("Adds a new product to the store.");

        products.MapPut("/{id:int}", (int id, CreateProductRequest request, IProductRepository repo) =>
        {
            if (repo.GetById(id) is null)
                return Results.NotFound();

            var product = new Product
            {
                Id = id,
                Name = request.Name,
                Price = request.Price,
                Category = request.Category
            };
            repo.Update(product);
            return Results.NoContent();
        })
            .WithName("UpdateProduct")
            .WithSummary("Update an existing product")
            .WithDescription("Updates the product with the given ID. Returns 404 if the product does not exist.");

        products.MapDelete("/{id:int}", (int id, IProductRepository repo) =>
        {
            if (repo.GetById(id) is null)
                return Results.NotFound();

            repo.Delete(id);
            return Results.NoContent();
        })
            .WithName("DeleteProduct")
            .WithSummary("Delete a product")
            .WithDescription("Removes the product with the given ID. Returns 404 if the product does not exist.");
    }

    private static ProductResponse ToResponse(Product p) =>
        new(p.Id, p.Name, p.Price, p.Category);
}
