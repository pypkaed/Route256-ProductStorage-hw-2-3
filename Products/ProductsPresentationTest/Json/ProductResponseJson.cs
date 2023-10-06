namespace ProductsPresentationTest.Json;

public class ProductResponseJson
{
    public ProductResponseJson() { }
    
    public ProductResponseJson(
        long id,
        string name,
        ProductGrpc.Decimal price,
        double weight,
        string category,
        string manufactureDate,
        long warehouseId
        )
    {
        Id = id;
        Name = name;
        Price = price;
        Weight = weight;
        Category = category;
        ManufactureDate = manufactureDate;
        WarehouseId = warehouseId;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public ProductGrpc.Decimal Price { get; set; }
    public double Weight { get; set; }
    public string Category { get; set; }
    public string ManufactureDate { get; set; }
    public long WarehouseId { get; set; }
}