using AutoMapper;
using ProductsDao.Dto;
using ProductsDao.Entities;
using ProductsDao.Models;

namespace ProductsDao.Repositories;

public class ProductInMemoryRepository : IProductRepository
{
    private readonly HashSet<Product> _products;
    private readonly IMapper _mapper;

    public ProductInMemoryRepository(HashSet<Product> products, IMapper mapper)
    {
        _products = products;
        _mapper = mapper;
    }

    public ProductDto Insert(Product product)
    {
        _products.Add(product);
        return _mapper.Map<ProductDto>(product);
    }

    public void DeleteById(ProductId productId)
    {
        var productDto = GetById(productId);
        var product = _mapper.Map<Product>(productDto);
        _products.Remove(product);
    }

    public ProductDto Update(Product product)
    {
        DeleteById(product.Id);
        Insert(product);
        
        return _mapper.Map<ProductDto>(product);
    }

    public ProductDto GetById(ProductId productId)
    {
        var product = FindProductById(productId);
        if (product is null)
        {
            throw new Exception($"Product with id {productId} does not exist.");
        }

        return _mapper.Map<ProductDto>(product);
    }

    private Product? FindProductById(ProductId productId)
    {
        return _products.FirstOrDefault(p => p.Id.Equals(productId));
    }
}