using ProductsBusiness.Dto;

namespace ProductsBusiness.Services;

public interface IProductService
{
    ProductDto CreateProduct(ProductDto productDto);
    ProductDto GetProductById(long id);
    void DeleteProductById(long id);
    ProductDto UpdateProductPrice(long id, decimal price);
    IReadOnlyCollection<ProductDto> GetProductsFiltered(FiltersDto filtersDto);
    IReadOnlyCollection<ProductDto> GetPage(int pageNum, int pageLength, IEnumerable<ProductDto> products);
}