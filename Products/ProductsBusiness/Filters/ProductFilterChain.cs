using ProductsDao.Entities;

namespace ProductsBusiness.Filters;

public class ProductFilterChain
{
    private const int FirstFilterIndex = 0;
    
    private readonly List<ProductFilter> _filters;

    public ProductFilterChain(List<ProductFilter> filters)
    {
        _filters = filters;

        _filters.Zip(_filters.Skip(1), (current, next) =>
        {
            current.SetNext(next);
            return next;
        });
    }

    public IReadOnlyCollection<ProductFilter> Filters => _filters;

    public IEnumerable<Product> Apply(IEnumerable<Product> products)
    {
        return _filters.Count == 0 ? products : _filters[FirstFilterIndex].Apply(products);
    }
}