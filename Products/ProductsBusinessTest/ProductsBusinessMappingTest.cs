using AutoMapper;
using FluentAssertions;
using ProductsBusiness.Profiles;
using ProductsDao.Models;
using Xunit;

namespace ProductsBusinessTest;

public class ProductsBusinessMappingTest
{
    private readonly IMapper _mapper;

    public ProductsBusinessMappingTest()
    {
        var mapperConfiguration = new MapperConfiguration(expression =>
        {
            expression.AddProfile<ProductProfile>();
        });

        _mapper = new Mapper(mapperConfiguration);
    }

    [Fact]
    public void ProductIdMappingTest()
    {
        var productIdValue = 5;
        var productId = new ProductId(productIdValue);

        var mappedProductId = _mapper.Map<ProductId>(productIdValue);
        
        mappedProductId.Should().Be(productId);
    }
}