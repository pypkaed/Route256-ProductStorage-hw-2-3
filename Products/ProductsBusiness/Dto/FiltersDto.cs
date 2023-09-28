namespace ProductsBusiness.Dto;

public class FiltersDto
{
    public string? ProductCategoryFilter { get; set; }
    public DateOnly? ProductManufactureDateFilter { get; set; }
    public long? ProductWarehouseIdFilter { get; set; }
}