namespace ProductsBusiness.Services;

public interface IProductService
{
    void CreateProduct(Product product);
    Product GetProductById(ProductId productId);
    void DeleteProductById(ProductId productId);
    Product UpdateProductPrice(ProductId productId, ProductPrice productPrice);
    List<Product> GetProductsFiltered(FilteredRequest filters);
}