namespace Products.Responses;

public record ProductResponse(
    long Id,
    string Name,
    decimal Price,
    double Weight,
    string Category,
    DateOnly ManufactureDate,
    long WarehouseId);