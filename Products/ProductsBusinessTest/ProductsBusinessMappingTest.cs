using AutoMapper;
using FluentAssertions;
using ProductsBusiness.Dto;
using ProductsBusiness.Profiles;
using ProductsDao.Entities;
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

    [Theory]
    [InlineData(123)]
    [InlineData(9999999)]
    [InlineData(1)]
    public void ProductIdMappingTest(long productIdValue)
    {
        var productId = new ProductId(productIdValue);

        var mappedProductId = _mapper.Map<ProductId>(productIdValue);
        
        mappedProductId.Should().Be(productId);
    }
    
    [Theory]
    [InlineData("Amogus")]
    [InlineData("Sus")]
    public void ProductNameMappingTest(string productNameValue)
    {
        var productName = new ProductName(productNameValue);

        var mappedProductName = _mapper.Map<ProductName>(productNameValue);
        
        mappedProductName.Should().Be(productName);
    }
    
    [Theory]
    [InlineData(1234567)]
    [InlineData(1)]
    [InlineData(1.231456786543)]
    public void ProductPriceMappingTest(decimal productPriceValue)
    {
        var productPrice = new ProductPrice(productPriceValue);

        var mappedProductPrice = _mapper.Map<ProductPrice>(productPriceValue);
        
        mappedProductPrice.Should().Be(productPrice);
    }
    
    [Theory]
    [InlineData(4.563)]
    [InlineData(0.000000001)]
    [InlineData(999999999999999999)]
    [InlineData(324567.3245678)]
    public void ProductWeightMappingTest(double productWeightValue)
    {
        var productWeight = new ProductWeight(productWeightValue);

        var mappedProductWeight = _mapper.Map<ProductWeight>(productWeightValue);
        
        mappedProductWeight.Should().Be(productWeight);
    }
    
    [Theory]
    [InlineData("Chemicals")]
    [InlineData("Electronics")]
    [InlineData("Food")]
    [InlineData("Other")]
    public void ProductCategoryMappingTest(string productCategory)
    {
        var mappedProductCategory = _mapper.Map<ProductCategory>(productCategory);
        
        mappedProductCategory.ToString().Should().Be(productCategory);
    }
    
    [Theory]
    [InlineData(23454)]
    [InlineData(1)]
    [InlineData(99999)]
    public void WarehouseIdMappingTest(long warehouseIdValue)
    {
        var warehouseId = new WarehouseId(warehouseIdValue);

        var mappedWarehouseId = _mapper.Map<WarehouseId>(warehouseIdValue);
        
        mappedWarehouseId.Should().Be(warehouseId);
    }
    
    [Theory]
    [MemberData(nameof(ProductDtoData))]
    public void ProductDtoToProductMappingTest(ProductDto productDto)
    {
        var product = new Product(
            _mapper.Map<ProductId>(productDto.Id),
            _mapper.Map<ProductName>(productDto.Name),
            _mapper.Map<ProductPrice>(productDto.Price),
            _mapper.Map<ProductWeight>(productDto.Weight),
            _mapper.Map<ProductCategory>(productDto.Category),
            productDto.ManufactureDate,
            _mapper.Map<WarehouseId>(productDto.WarehouseId));
        
        var mappedProductId = _mapper.Map<Product>(productDto);
        
        mappedProductId.Should().Be(product);
    }
    
    public static IEnumerable<object[]> ProductDtoData()
    {
        yield return new object[]
        {
            new ProductDto(
                Id: 1,
                Name: "Pomogit",
                Price: 12345m,
                Weight: 15.2,
                Category: "Food",
                ManufactureDate: DateOnly.MinValue,
                WarehouseId: 12345),
        };
    }
    
    //TODO: exceptions tests
}