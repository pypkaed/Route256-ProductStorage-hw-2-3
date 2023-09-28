using ProductsDao.Entities;

namespace ProductsBusiness.Filters;

public class ProductFilterChain
{
    private const int FirstFilterIndex = 0;
    
    private readonly List<ProductFilter> _filters;

    public ProductFilterChain(List<ProductFilter> filters)
    {
        _filters = filters;
        for (int i = 0; i < _filters.Count - 1; i++)
        {
            _filters[i].SetNext(filters[i + 1]);
        }
    }

    public IReadOnlyCollection<ProductFilter> Filters => _filters;

    public IEnumerable<Product> Apply(IEnumerable<Product> products)
    {
        return _filters.Count == 0 ? products : _filters[FirstFilterIndex].Apply(products);
    }
}