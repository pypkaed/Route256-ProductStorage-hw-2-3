using AutoMapper;
using ProductsBusiness.Dto;
using ProductsDao.Entities;

namespace ProductsBusiness.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
    }
}