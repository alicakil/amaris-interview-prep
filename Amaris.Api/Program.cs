using Amaris.Core.Models;
using Amaris.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRepository<Product>, InMemoryRepository<Product>>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var products = app.MapGroup("/api/products")
    .WithTags("Products");

products.MapGet("/", (IRepository<Product> repo) =>
    Results.Ok(repo.GetAll()))
    .WithName("GetAllProducts")
    .WithSummary("Get all products")
    .WithDescription("Returns a list of all products in the store.");

products.MapGet("/{id:int}", (int id, IRepository<Product> repo) =>
    repo.GetById(id) is { } product
        ? Results.Ok(product)
        : Results.NotFound())
    .WithName("GetProductById")
    .WithSummary("Get a product by ID")
    .WithDescription("Returns a single product matching the given ID, or 404 if not found.");

products.MapPost("/", (Product product, IRepository<Product> repo) =>
{
    repo.Add(product);
    return Results.Created($"/api/products/{product.Id}", product);
})
    .WithName("CreateProduct")
    .WithSummary("Create a new product")
    .WithDescription("Adds a new product to the store. The product ID must be unique.");

products.MapPut("/{id:int}", (int id, Product product, IRepository<Product> repo) =>
{
    if (repo.GetById(id) is null)
        return Results.NotFound();

    product.Id = id;
    repo.Update(product);
    return Results.NoContent();
})
    .WithName("UpdateProduct")
    .WithSummary("Update an existing product")
    .WithDescription("Updates the product with the given ID. Returns 404 if the product does not exist.");

products.MapDelete("/{id:int}", (int id, IRepository<Product> repo) =>
{
    if (repo.GetById(id) is null)
        return Results.NotFound();

    repo.Delete(id);
    return Results.NoContent();
})
    .WithName("DeleteProduct")
    .WithSummary("Delete a product")
    .WithDescription("Removes the product with the given ID. Returns 404 if the product does not exist.");

app.Run();
