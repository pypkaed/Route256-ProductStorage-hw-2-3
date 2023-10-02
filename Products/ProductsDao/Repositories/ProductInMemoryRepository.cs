using System.Collections.Concurrent;
using ProductsDao.Entities;
using ProductsDao.Exceptions;
using ProductsDao.Models;

namespace ProductsDao.Repositories;

public class ProductInMemoryRepository : IProductRepository
{
    private readonly ConcurrentDictionary<ProductId, Product> _products;

    public ProductInMemoryRepository()
    {
        _products = new ConcurrentDictionary<ProductId, Product>();
    }

    public Product Insert(Product product)
    {
        if (!_products.TryAdd(product.Id, product))
        {
            throw RepositoryException.ProductAlreadyExists(product.Id);
        }
        
        return product;
    }

    public void DeleteById(ProductId productId)
    {
        var product = GetById(productId);
        _products.Remove(product.Id, out var removedProduct);
    }

    public Product Update(Product product)
    {
        var oldProduct = GetById(product.Id);
        _products.TryUpdate(product.Id, product, oldProduct);
        
        return product;
    }

    public Product GetById(ProductId productId)
    {
        var product = FindProductById(productId);
        if (product is null)
        {
            throw RepositoryException.ProductDoesNotExists(productId);
        }

        return product;
    }

    public IReadOnlyDictionary<ProductId, Product> GetAll()
    {
        return _products.AsReadOnly();
    }

    private Product? FindProductById(ProductId productId)
    {
        return _products[productId];
    }
}