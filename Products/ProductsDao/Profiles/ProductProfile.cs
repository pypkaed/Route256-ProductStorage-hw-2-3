using AutoMapper;
using ProductsDao.Dto;
using ProductsDao.Entities;

namespace ProductsDao.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
    }
}