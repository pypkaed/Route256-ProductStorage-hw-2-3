using AutoMapper;
using ProductsBusiness.Dto;
using ProductsDao.Entities;
using ProductsDao.Models;

namespace ProductsBusiness.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductId, long>().ConstructUsing(input => input.Id);
        CreateMap<ProductName, string>().ConstructUsing(input => input.Name);
        CreateMap<ProductPrice, decimal>().ConstructUsing(input => input.Price);
        CreateMap<ProductWeight, double>().ConstructUsing(input => input.Weight);
        CreateMap<ProductCategory, string>().ConstructUsing(input => input.ToString());
        CreateMap<WarehouseId, long>().ConstructUsing(input => input.Id);
        CreateMap<Product, ProductDto>();
        
        
        CreateMap<string, ProductCategory>().ConstructUsing(input => Enum.Parse<ProductCategory>(input));
        CreateMap<ProductDto, Product>();
    }
}