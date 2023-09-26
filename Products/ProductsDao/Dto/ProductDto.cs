using ProductsDao.Models;

namespace ProductsDao.Dto;

public record ProductDto(
    ProductId Id,
    ProductName Name,
    ProductPrice Price,
    ProductWeight Weight,
    ProductCategory Category,
    DateTime ManufactureDate,
    WarehouseId WarehouseId);