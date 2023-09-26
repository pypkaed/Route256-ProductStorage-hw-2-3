using Products.Entities;
using Products.Models;

namespace Products.Repositories;

public interface IProductRepository
{
    void Insert(Product product);
    void DeleteById(ProductId productId);
    public Product Update(Product product);
    Product GetById(ProductId productId);
}