using ProductsDao.Entities;
using ProductsDao.Models;

namespace ProductsBusiness.Filters;

public class ProductCategoryFilter : ProductFilter
{
    private readonly ProductCategory _productCategory;
    
    public ProductCategoryFilter(ProductCategory productCategory)
    {
        _productCategory = productCategory;
    }
    
    public override IEnumerable<Product> Apply(IEnumerable<Product> products)
    {
        var filtered = products.Where(p => p.Category.Equals(_productCategory));
        return Next is null ? filtered : Next.Apply(filtered);
    }
}