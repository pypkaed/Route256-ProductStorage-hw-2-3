using AutoMapper;
using ProductsBusiness.Dto;

namespace Products.Profiles;

public class RequestProfile : Profile
{
    private const decimal NanoFactor = 1_000_000_000;
    public RequestProfile()
    {
        CreateMap<string, DateOnly>().ConstructUsing(x => DateOnly.Parse(x));
        CreateMap<ProductGrpc.Decimal, decimal>().ConstructUsing(x => x.Units + x.Nanos / NanoFactor);
        
        CreateMap<ProductGrpc.CreateProductRequest, ProductDto>();
        
        CreateMap<ProductGrpc.GetProductsFilteredRequest, FiltersDto>();
    }
}