using AutoMapper;
using ProductsBusiness.Dto;
using ProductsBusiness.Profiles;
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

    public ProductDto CreateProduct(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        
        _repository.Insert(product);

        return _mapper.Map<ProductDto>(product);
    }

    public ProductDto GetProductById(long id)
    {
        var productId = new ProductId(id);
        var product = _repository.GetById(productId);
        
        return _mapper.Map<ProductDto>(product);
    }

    public void DeleteProductById(long id)
    {
        var productId = new ProductId(id);
        _repository.DeleteById(productId);
    }

    public ProductDto UpdateProductPrice(long id, decimal price)
    {
        var productId = new ProductId(id);
        var productPrice = new ProductPrice(price);

        var product = _repository.GetById(productId);
        product.ChangePrice(productPrice);
        
        _repository.Update(product);
        
        return _mapper.Map<ProductDto>(product);
    }

    public List<ProductDto> GetProductsFiltered(FiltersDto filtersDto)
    {
        var filterChain = filtersDto.AsProductFilterChain();

        var products = _repository.GetAll();
        var filteredProducts = filterChain.Apply(products);

        var result = filteredProducts.Select(p => _mapper.Map<ProductDto>(p));

        return result.ToList();
    }

    public List<ProductDto> GetPage(int pageNum, int pageLength, List<ProductDto> products)
    {
        var skipPagesNum = (pageNum - 1) * pageLength;
        
        return products.Skip(skipPagesNum).Take(pageLength).ToList();
    }
}