using ProductsDao.Entities;
using ProductsDao.Models;

namespace ProductsDao.Repositories;

public interface IProductRepository
{
    Product Insert(Product product);
    void DeleteById(ProductId productId);
    Product Update(Product product);
    Product GetById(ProductId productId);
}