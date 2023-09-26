using ProductsDao.Dto;
using ProductsDao.Entities;
using ProductsDao.Models;

namespace ProductsDao.Repositories;

public interface IProductRepository
{
    ProductDto Insert(Product product);
    void DeleteById(ProductId productId);
    ProductDto Update(Product product);
    ProductDto GetById(ProductId productId);
}