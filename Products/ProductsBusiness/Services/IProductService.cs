using ProductsBusiness.Dto;
using ProductsDao.Entities;
using ProductsDao.Models;

namespace ProductsBusiness.Services;

public interface IProductService
{
    ProductDto CreateProduct(
        ProductId id,
        ProductName name,
        ProductPrice price,
        ProductWeight weight,
        ProductCategory category,
        DateTime manufactureDate,
        WarehouseId warehouseId);
    ProductDto GetProductById(ProductId productId);
    void DeleteProductById(ProductId productId);
    ProductDto UpdateProductPrice(ProductId productId, ProductPrice productPrice);
    // List<ProductDto> GetProductsFiltered(FilteredRequest filters);
}