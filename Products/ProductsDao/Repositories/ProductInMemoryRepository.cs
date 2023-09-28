using ProductsDao.Entities;
using ProductsDao.Models;

namespace ProductsDao.Repositories;

public class ProductInMemoryRepository : IProductRepository
{
    private readonly List<Product> _products;

    public ProductInMemoryRepository()
    {
        _products = new List<Product>();
    }

    public Product Insert(Product product)
    {
        if (_products.Contains(product))
        {
            // TODO: execpton
            throw new Exception("product already exists");
        }
        _products.Add(product);
        return product;
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
            // TODO: excption
            throw new Exception($"Product with id {productId} does not exist.");
        }

        return product;
    }

    public IReadOnlyCollection<Product> GetAll()
    {
        return _products.AsReadOnly();
    }

    private Product? FindProductById(ProductId productId)
    {
        return _products.FirstOrDefault(p => p.Id.Equals(productId));
    }
}