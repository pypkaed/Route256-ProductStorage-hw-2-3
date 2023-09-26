using Products.Entities;
using Products.Models;

namespace Products.Repositories;

public class ProductInMemoryRepository : IProductRepository
{
    private readonly HashSet<Product> _products;

    public ProductInMemoryRepository(HashSet<Product> products)
    {
        _products = products;
    }

    public void Insert(Product product)
    {
        _products.Add(product);
    }

    public void DeleteById(ProductId productId)
    {
        var product = GetById(productId);
        _products.Remove(product);
    }

    public Product Update(Product product)
    {
        DeleteById(product.Id);
        Insert(product);
        
        return product;
    }

    public Product GetById(ProductId productId)
    {
        var product = FindProductById(productId);
        if (product is null)
        {
            throw new Exception($"Product with id {productId} does not exist.");
        }

        return product;
    }

    private Product? FindProductById(ProductId productId)
    {
        return _products.FirstOrDefault(p => p.Id.Equals(productId));
    }
}