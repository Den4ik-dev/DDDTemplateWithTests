namespace Contracts.Product;

public record CreateProductDto(string Name, string Description, int Price, Guid CategoryId);
