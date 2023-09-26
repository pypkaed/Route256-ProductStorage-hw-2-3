using ProductsDao.Entities;
using ProductsDao.Models;

namespace ProductsDao.Repositories;

public interface IProductRepository
{
    void Insert(Product product);
    void DeleteById(ProductId productId);
    public Product Update(Product product);
    Product GetById(ProductId productId);
}