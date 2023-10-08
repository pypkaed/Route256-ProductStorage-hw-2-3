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
    public void ProductIdMapping_Success(long productIdValue)
    {
        // Arrange
        var actual = new ProductId(productIdValue);

        // Act
        var expected = _mapper.Map<ProductId>(productIdValue);

        // Assert
        Assert.Equal(expected, actual);
    } 
    
    [Theory]
    [InlineData("Amogus")]
    [InlineData("Sus")]
    public void ProductNameMapping_Success(string productNameValue)
    {
        // Arrange
        var actual = new ProductName(productNameValue);

        // Act
        var expected = _mapper.Map<ProductName>(productNameValue);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(1234567)]
    [InlineData(1)]
    [InlineData(1.231456786543)]
    public void ProductPriceMapping_Success(decimal productPriceValue)
    {
        // Arrange
        var actual = new ProductPrice(productPriceValue);

        // Act
        var expected = _mapper.Map<ProductPrice>(productPriceValue);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(4.563)]
    [InlineData(0.000000001)]
    [InlineData(999999999999999999)]
    [InlineData(324567.3245678)]
    public void ProductWeightMapping_Success(double productWeightValue)
    {
        // Arrange
        var actual = new ProductWeight(productWeightValue);

        // Act
        var expected = _mapper.Map<ProductWeight>(productWeightValue);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData("Chemicals")]
    [InlineData("Electronics")]
    [InlineData("Food")]
    [InlineData("Other")]
    public void ProductCategoryMapping_Success(string productCategory)
    {
        // Act
        var expected = _mapper.Map<ProductCategory>(productCategory);
        
        // Assert
        Assert.Equal(expected.ToString(), productCategory);
    }
    
    [Theory]
    [InlineData(23454)]
    [InlineData(1)]
    [InlineData(99999)]
    public void WarehouseIdMapping_Success(long warehouseIdValue)
    {
        // Arrange
        var actual = new WarehouseId(warehouseIdValue);

        // Act
        var expected = _mapper.Map<WarehouseId>(warehouseIdValue);
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [MemberData(nameof(ProductDtoData))]
    public void ProductDtoToProductMapping_Success(ProductDto productDto)
    {
        // Arrange
        var actual = new Product(
            _mapper.Map<ProductId>(productDto.Id),
            _mapper.Map<ProductName>(productDto.Name),
            _mapper.Map<ProductPrice>(productDto.Price),
            _mapper.Map<ProductWeight>(productDto.Weight),
            _mapper.Map<ProductCategory>(productDto.Category),
            productDto.ManufactureDate,
            _mapper.Map<WarehouseId>(productDto.WarehouseId));
        
        // Act
        var expected = _mapper.Map<Product>(productDto);
        
        // Assert
        Assert.Equal(expected, actual);
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
}