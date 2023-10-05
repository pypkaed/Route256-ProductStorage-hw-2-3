namespace ProductsBusiness.Dto;

public class FiltersDto : IEquatable<FiltersDto>
{
    public string? ProductCategoryFilter { get; set; }
    public DateOnly? ProductManufactureDateFilter { get; set; }
    public long? ProductWarehouseIdFilter { get; set; }

    public bool Equals(FiltersDto? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ProductCategoryFilter == other.ProductCategoryFilter
               && Nullable.Equals(ProductManufactureDateFilter, 
                   other.ProductManufactureDateFilter)
               && ProductWarehouseIdFilter == other.ProductWarehouseIdFilter;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FiltersDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ProductCategoryFilter, ProductManufactureDateFilter, ProductWarehouseIdFilter);
    }
}