using ProductsDao.Models;

namespace ProductsBusiness.Dto;

public record ProductDto(
    ProductId Id,
    ProductName Name,
    ProductPrice Price,
    ProductWeight Weight,
    ProductCategory Category,
    DateTime ManufactureDate,
    WarehouseId WarehouseId);