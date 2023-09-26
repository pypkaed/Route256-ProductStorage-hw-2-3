using Products.Entities;
using Products.Models;
using Products.Repositories;
using Products.Requests;

namespace Products.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public void CreateProduct(Product product)
    {
        _repository.Insert(product);
    }

    public Product GetProductById(ProductId productId)
    {
        return _repository.GetById(productId);
    }

    public void DeleteProductById(ProductId productId)
    {
        _repository.DeleteById(productId);
    }

    public Product UpdateProductPrice(ProductId productId, ProductPrice productPrice)
    {
        var product = _repository.GetById(productId);
        product.ChangePrice(productPrice);
        _repository.Update(product);

        return product;
    }

    public List<Product> GetProductsFiltered(FilteredRequest filters)
    {
        throw new NotImplementedException();
    }
}