using System.Text.Json.Serialization;
using Decimal = ProductGrpc.Decimal;

namespace ProductsPresentationTest.Json;

public class CreateProductRequestJson
{
    public CreateProductRequestJson()
    {
    }

    public CreateProductRequestJson(
        long id,
        string name,
        ProductGrpc.Decimal price,
        double weight,
        ProductGrpc.ProductCategory category,
        string manufactureDate,
        long warehouseId)
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
    public Decimal Price { get; set; }
    public double Weight { get; set; }
    public ProductGrpc.ProductCategory Category { get; set; }
    [JsonPropertyName("manufacture_date")]
    public string ManufactureDate { get; set; }
    [JsonPropertyName("warehouse_id")]
    public long WarehouseId { get; set; }
}