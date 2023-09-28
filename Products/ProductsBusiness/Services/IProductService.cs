using ProductsBusiness.Dto;
using ProductsBusiness.Filters;

namespace ProductsBusiness.Services;

public interface IProductService
{
    ProductDto CreateProduct(ProductDto productDto);
    ProductDto GetProductById(long id);
    void DeleteProductById(long id);
    ProductDto UpdateProductPrice(long id, decimal price);
    List<ProductDto> GetProductsFiltered(FiltersDto filtersDto);
    List<ProductDto> GetPage(int pageNum, int pageLength, List<ProductDto> products);
}