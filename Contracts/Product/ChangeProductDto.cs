namespace Contracts.Product;

public record ChangeProductDto(string Name, string Description, int Price, Guid CategoryId);
