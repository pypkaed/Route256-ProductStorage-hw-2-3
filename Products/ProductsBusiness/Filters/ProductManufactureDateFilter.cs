using ProductsDao.Entities;

namespace ProductsBusiness.Filters;

public class ProductManufactureDateFilter : ProductFilter
{
    private readonly DateOnly _manufactureDate;

    public ProductManufactureDateFilter(DateOnly manufactureDate)
    {
        _manufactureDate = manufactureDate;
    }

    public override IEnumerable<Product> Apply(IEnumerable<Product> products)
    {
        var filtered = products.Where(p => p.ManufactureDate.Equals(_manufactureDate));
        return Next is null ? filtered : Next.Apply(filtered);
    }
}