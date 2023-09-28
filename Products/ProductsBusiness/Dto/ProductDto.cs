namespace ProductsBusiness.Dto;

public record ProductDto(
    long Id,
    string Name,
    decimal Price,
    double Weight,
    string Category,
    DateTime ManufactureDate,
    long WarehouseId);