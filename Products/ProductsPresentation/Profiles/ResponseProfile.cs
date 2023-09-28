using AutoMapper;
using Products.Responses;
using ProductsBusiness.Dto;

namespace Products.Profiles;

public class ResponseProfile : Profile
{
    public ResponseProfile()
    {
        CreateMap<ProductDto, ProductResponse>();
        CreateMap<List<ProductDto>, GetProductsFilteredResponse>();
    }
}