using AutoMapper;
using Products.Requests;
using ProductsBusiness.Dto;

namespace Products.Profiles;

public class RequestProfile : Profile
{
    public RequestProfile()
    {
        CreateMap<string, DateOnly>().ConstructUsing(x => DateOnly.Parse(x));
        
        CreateMap<CreateProductRequest, ProductDto>();
        CreateMap<GetProductsFilteredRequest, FiltersDto>();
        
        CreateMap<ProductGrpc.CreateProductRequest, ProductDto>()
            .ForMember(productDto => productDto.Price,
                expr =>
                    expr.MapFrom(productRequest => (decimal) productRequest.Price));
        
        CreateMap<ProductGrpc.GetProductsFilteredRequest, FiltersDto>();
    }
}