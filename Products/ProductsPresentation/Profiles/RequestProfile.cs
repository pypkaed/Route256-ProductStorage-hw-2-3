using AutoMapper;
using Products.Requests;
using ProductsBusiness.Dto;

namespace Products.Profiles;

public class RequestProfile : Profile
{
    public RequestProfile()
    {
        CreateMap<CreateProductRequest, ProductDto>();
        CreateMap<GetProductsFilteredRequest, FiltersDto>();
    }
}