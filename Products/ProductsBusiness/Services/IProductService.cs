using ProductsBusiness.Dto;

namespace ProductsBusiness.Services;

public interface IProductService
{
    ProductDto CreateProduct(
        long id,
        string name,
        decimal price,
        double weight,
        string category,
        DateTime manufactureDate,
        long warehouseId);
    ProductDto GetProductById(long id);
    void DeleteProductById(long id);
    ProductDto UpdateProductPrice(long id, decimal price);
    // List<ProductDto> GetProductsFiltered(FilteredRequest filters);
}