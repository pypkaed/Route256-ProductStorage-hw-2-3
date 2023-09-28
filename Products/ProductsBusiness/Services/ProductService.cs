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

    public ProductService(
        IProductRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public ProductDto CreateProduct(
        long id,
        string name,
        decimal price,
        double weight,
        string category,
        DateTime manufactureDate,
        long warehouseId)
    {
        var productId = new ProductId(id);
        var productName = new ProductName(name);
        var productPrice = new ProductPrice(price);
        var productWeight = new ProductWeight(weight);
        var warehouseIdModel = new WarehouseId(warehouseId);
        if (!Enum.TryParse<ProductCategory>(category, ignoreCase: true, out var productCategory))
        {
            // TODO: exception
            throw new Exception();
        }

        var product = new Product(
            productId,
            productName,
            productPrice,
            productWeight,
            productCategory,
            manufactureDate,
            warehouseIdModel);
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