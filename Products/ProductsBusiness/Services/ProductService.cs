using AutoMapper;
using ProductsBusiness.Dto;
using ProductsDao.Entities;
using ProductsDao.Models;
using ProductsDao.Repositories;

namespace ProductsBusiness.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public ProductDto CreateProduct(
        ProductId id,
        ProductName name,
        ProductPrice price,
        ProductWeight weight,
        ProductCategory category,
        DateTime manufactureDate,
        WarehouseId warehouseId)
    {
        var product = new Product(id, name, price, weight, category, manufactureDate, warehouseId);
        _repository.Insert(product);

        return _mapper.Map<ProductDto>(product);
    }

    public ProductDto GetProductById(ProductId productId)
    {
        var product = _repository.GetById(productId);
        
        return _mapper.Map<ProductDto>(product);
    }

    public void DeleteProductById(ProductId productId)
    {
        _repository.DeleteById(productId);
    }

    public ProductDto UpdateProductPrice(ProductId productId, ProductPrice productPrice)
    {
        var product = _repository.GetById(productId);
        product.ChangePrice(productPrice);
        
        _repository.Update(product);
        
        return _mapper.Map<ProductDto>(product);
    }
}