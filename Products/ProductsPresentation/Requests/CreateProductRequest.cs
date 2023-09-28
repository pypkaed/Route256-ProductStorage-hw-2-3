namespace Products.Requests;

public record CreateProductRequest(
    long Id,
    string Name,
    decimal Price,
    double Weight,
    string Category,
    DateOnly ManufactureDate,
    long WarehouseId);