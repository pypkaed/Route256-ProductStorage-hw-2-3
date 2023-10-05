using AutoMapper;
using ProductGrpc;
using Products.Profiles;
using ProductsBusiness.Dto;
using Xunit;

namespace ProductsPresentationTest;

public class PresentationMappingTest
{
    private const decimal NanoFactor = 1_000_000_000;
    
    private readonly IMapper _mapper;

    public PresentationMappingTest()
    {
        var mapperConfiguration = new MapperConfiguration(expression =>
        {
            expression.AddProfile<RequestProfile>();
            expression.AddProfile<ResponseProfile>();
        });

        _mapper = new Mapper(mapperConfiguration);
    }

    [Theory]
    [MemberData(nameof(ProductGrpcDecimalData))]
    public void GrpcDecimalToDecimalMappingTest(ProductGrpc.Decimal grpcDecimal)
    {
        var actualDecimal = grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;

        var mappedDecimal = _mapper.Map<decimal>(grpcDecimal);
        
        Assert.Equal(actualDecimal, mappedDecimal);
    }

    [Theory]
    [InlineData(1234567.32456)]
    [InlineData(0.0000000001)]
    public void DecimalToGrpcDecimalMappingTest(decimal decimalValue)
    {
        var grpcDecimal = new ProductGrpc.Decimal
        {
            Units = decimal.ToInt64(decimalValue),
            Nanos = decimal.ToInt32((decimalValue - decimal.Truncate(decimalValue)) * NanoFactor),
        };

        var mappedDecimal = _mapper.Map<ProductGrpc.Decimal>(decimalValue);

        Assert.Equal(grpcDecimal, mappedDecimal);
    }

    public static IEnumerable<object[]> ProductGrpcDecimalData()
    {
        yield return new object[]
        {
            new ProductGrpc.Decimal
            {
                Units = 123,
                Nanos = 123456789,
            }
        };
        yield return new object[]
        {
            new ProductGrpc.Decimal
            {
                Units = 123,
                Nanos = 120000000,
            }
        };
    }

    [Theory]
    [MemberData(nameof(CreateProductRequestData))]
    public void CreateProductRequestToProductDtoMappingTest(CreateProductRequest request)
    {
        var productDto = new ProductDto(
            request.Id,
            request.Name, 
            _mapper.Map<decimal>(request.Price),
            request.Weight,
            request.Category.ToString(),
            DateOnly.Parse(request.ManufactureDate),
            request.WarehouseId);

        var mappedProductDto = _mapper.Map<ProductDto>(request);
        
        Assert.Equal(productDto, mappedProductDto);
    }
    
    public static IEnumerable<object[]> CreateProductRequestData()
    {
        yield return new object[]
        {
            new CreateProductRequest()
            {
                Id = 1,
                Name = "Bebrijin",
                Price = new ProductGrpc.Decimal()
                {
                    Units = 123,
                    Nanos = 100000000,
                },
                Weight = 3245.4,
                Category = ProductCategory.Electronics,
                ManufactureDate = "2022-09-01",
                WarehouseId = 12345,
            }
        };
    }
    
    [Theory]
    [MemberData(nameof(GetProductsFilteredRequestData))]
    public void GetProductsFilteredRequestToFiltersDtoMappingTest(GetProductsFilteredRequest request)
    {
        var filtersDto = new FiltersDto
        {
            ProductCategoryFilter = request.ProductCategoryFilter, 
            ProductManufactureDateFilter = DateOnly.Parse(request.ProductManufactureDateFilter),
            ProductWarehouseIdFilter = request.ProductWarehouseIdFilter,
        };
        
        var mappedFiltersDto = _mapper.Map<FiltersDto>(request);
        
        Assert.Equal(filtersDto, mappedFiltersDto);
    }
    
    public static IEnumerable<object[]> GetProductsFilteredRequestData()
    {
        yield return new object[]
        {
            new GetProductsFilteredRequest()
            {
                ProductManufactureDateFilter = "2022/11/11",
                ProductCategoryFilter = "Food",
                ProductWarehouseIdFilter = 123
            }
        };
    }

    [Theory]
    [MemberData(nameof(ProductDtoData))]
    public void ProductDtoToProductResponseMappingTest(ProductDto productDto)
    {
        var productResponse = new ProductResponse()
        {   
            Id = productDto.Id,
            Name = productDto.Name,
            Price = _mapper.Map<ProductGrpc.Decimal>(productDto.Price),
            Weight = productDto.Weight,
            Category = _mapper.Map<ProductCategory>(productDto.Category),
            ManufactureDate = productDto.ManufactureDate.ToString(),
            WarehouseId = productDto.WarehouseId,
        };

        var mappedProductResponse = _mapper.Map<ProductResponse>(productDto);
        
        Assert.Equal(productResponse, mappedProductResponse);
    }

    public static IEnumerable<object[]> ProductDtoData()
    {
        yield return new object[]
        {
            new ProductDto(
                1,
                "AaaaaAAA", 
                12345.234m,
                1234.3,
                "Food",
                DateOnly.FromDateTime(DateTime.Now),
                1234)
        };
    }
    
    // TODO: exceptions tets :(
}