using ProductsDao.Entities;
using ProductsDao.Models;

namespace ProductsBusiness.Filters;

public class ProductWarehouseIdFilter : ProductFilter
{
    private readonly WarehouseId _warehouseId;

    public ProductWarehouseIdFilter(WarehouseId warehouseId)
    {
        _warehouseId = warehouseId;
    }
    public override IEnumerable<Product> Apply(IEnumerable<Product> products)
    {
        var filtered = products.Where(p => p.WarehouseId.Equals(_warehouseId));
        return Next is null ? filtered : Next.Apply(filtered);
    }
}