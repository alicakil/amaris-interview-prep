using Amaris.Api.Endpoints;
using Amaris.Api.Middleware;
using Amaris.Core.Repositories;
using Amaris.Data;
using Amaris.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<RequestHeaderLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

SeedData.Initialize(app.Services.GetRequiredService<IProductRepository>());

app.MapProductEndpoints();

app.Run();
