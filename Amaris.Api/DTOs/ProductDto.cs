namespace Amaris.Api.DTOs;

public record CreateProductRequest(string Name, decimal Price, string Category);
public record ProductResponse(int Id, string Name, decimal Price, string Category);
