using Amaris.Core.Models;
using Amaris.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRepository<Product>, InMemoryRepository<Product>>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var products = app.MapGroup("/api/products");

products.MapGet("/", (IRepository<Product> repo) =>
    Results.Ok(repo.GetAll()));

products.MapGet("/{id:int}", (int id, IRepository<Product> repo) =>
    repo.GetById(id) is { } product
        ? Results.Ok(product)
        : Results.NotFound());

products.MapPost("/", (Product product, IRepository<Product> repo) =>
{
    repo.Add(product);
    return Results.Created($"/api/products/{product.Id}", product);
});

products.MapPut("/{id:int}", (int id, Product product, IRepository<Product> repo) =>
{
    if (repo.GetById(id) is null)
        return Results.NotFound();

    product.Id = id;
    repo.Update(product);
    return Results.NoContent();
});

products.MapDelete("/{id:int}", (int id, IRepository<Product> repo) =>
{
    if (repo.GetById(id) is null)
        return Results.NotFound();

    repo.Delete(id);
    return Results.NoContent();
});

app.Run();
