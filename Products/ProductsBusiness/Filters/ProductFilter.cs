using ProductsDao.Entities;

namespace ProductsBusiness.Filters;

public abstract class ProductFilter
{
    public ProductFilter? Next { get; private set; }

    public ProductFilter SetNext(ProductFilter? nextFilter)
    {
        Next = nextFilter;
        return Next;
    }
    public abstract IEnumerable<Product> Apply(IEnumerable<Product> products);
}