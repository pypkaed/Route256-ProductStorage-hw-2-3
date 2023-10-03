using ProductsDao.Models;

namespace ProductsDao.Entities;

public class Product : IEquatable<Product>
{
    public Product(
        ProductId id,
        ProductName name,
        ProductPrice price,
        ProductWeight weight,
        ProductCategory category,
        DateOnly manufactureDate,
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
    public ProductPrice Price { get; private set; }
    public ProductWeight Weight { get; }
    public ProductCategory Category { get; }
    public DateOnly ManufactureDate { get; init; }
    public WarehouseId WarehouseId { get; }

    public void ChangePrice(ProductPrice newPrice)
    {
        Price = newPrice;
    }

    public bool Equals(Product? other)
    {
        return Id.Equals(other?.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Product);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}