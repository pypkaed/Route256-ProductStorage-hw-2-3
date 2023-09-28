namespace Products.Requests;

public class GetProductsFilteredRequest
{
    public string? ProductCategoryFilter { get; set; }
    public DateOnly? ProductManufactureDateFilter { get; set; }
    public long? ProductWarehouseIdFilter { get; set; }
}