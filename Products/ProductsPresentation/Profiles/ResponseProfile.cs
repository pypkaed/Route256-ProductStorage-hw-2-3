using AutoMapper;
using ProductsBusiness.Dto;

namespace Products.Profiles;

public class ResponseProfile : Profile
{
    private const decimal NanoFactor = 1_000_000_000;

    public ResponseProfile()
    {
        CreateMap<decimal, ProductGrpc.Decimal>()
            .ForMember(grpcDecimal => grpcDecimal.Units, 
                o =>
                    o.MapFrom(src => decimal.ToInt64(src)))
            .ForMember(grpcDecimal => grpcDecimal.Nanos, 
                o =>
                    o.MapFrom(src => decimal.ToInt32((src - decimal.Truncate(src)) * NanoFactor)));

        CreateMap<ProductDto, ProductGrpc.ProductResponse>();
    }
}