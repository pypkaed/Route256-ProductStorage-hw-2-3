using Products.Models;

namespace Products.Entities;

public class Product
{
    public Product(
        ProductId id,
        ProductName name,
        ProductPrice price,
        ProductWeight weight,
        ProductCategory category,
        DateTime manufactureDate,
        WarehouseId warehouseId)
    {
        Id = id;
        Name = name;
        Price = price;
        Weight = weight;
        Category = category;
        ManufactureDate = manufactureDate;
        WarehouseId = warehouseId;
    }

    public ProductId Id { get; init; }
    public ProductName Name { get; init; }
    public ProductPrice Price { get; }
    public ProductWeight Weight { get; }
    public ProductCategory Category { get; }
    public DateTime ManufactureDate { get; init; }
    public WarehouseId WarehouseId { get; }
}