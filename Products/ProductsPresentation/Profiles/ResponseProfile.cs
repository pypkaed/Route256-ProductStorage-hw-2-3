using AutoMapper;
using Products.Responses;
using ProductsBusiness.Dto;

namespace Products.Profiles;

public class ResponseProfile : Profile
{
    public ResponseProfile()
    {
        CreateMap<ProductDto, CreateProductResponse>();
        CreateMap<CreateProductResponse, ProductDto>();
    }
}