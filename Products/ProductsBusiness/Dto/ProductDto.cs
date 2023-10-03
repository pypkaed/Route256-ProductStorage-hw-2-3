namespace ProductsBusiness.Dto;

public record ProductDto(
    long Id,
    string Name,
    decimal Price,
    double Weight,
    string Category,
    DateOnly ManufactureDate,
    long WarehouseId);