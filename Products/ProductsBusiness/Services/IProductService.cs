using ProductsBusiness.Dto;
using ProductsDao.Entities;
using ProductsDao.Models;

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
    ProductDto GetProductById(ProductId productId);
    void DeleteProductById(ProductId productId);
    ProductDto UpdateProductPrice(ProductId productId, ProductPrice productPrice);
    // List<ProductDto> GetProductsFiltered(FilteredRequest filters);
}