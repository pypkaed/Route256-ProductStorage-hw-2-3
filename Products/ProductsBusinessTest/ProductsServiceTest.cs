using AutoMapper;
using Moq;
using ProductsBusiness.Dto;
using ProductsBusiness.Profiles;
using ProductsBusiness.Services;
using ProductsDao.Entities;
using ProductsDao.Exceptions;
using ProductsDao.Models;
using ProductsDao.Repositories;
using Xunit;

namespace ProductsBusinessTest;

public class ProductsServiceTest
{
    private readonly IMapper _mapper;
    
    public ProductsServiceTest()
    {
        var mapperConfiguration = new MapperConfiguration(expression =>
        {
            expression.AddProfile<ProductProfile>();
        });
        _mapper = new Mapper(mapperConfiguration);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    [InlineData(12345)]
    public void GetProductById(long id)
    {
        // Arrange
        var repositoryMock = new Mock<IProductRepository>();
        repositoryMock.Setup(repository => repository.GetById(It.IsAny<ProductId>()))
            .Returns<ProductId>(id =>
                InitializeProduct(id.Id,
                    "SomeName",
                    123.2m,
                    1.42,
                    ProductCategory.Chemicals,
                    DateOnly.MinValue,
                    234));
        
        var service = new ProductService(repositoryMock.Object, _mapper);

        // Act
        service.GetProductById(id);
        
        // Assert
        repositoryMock.Verify(repository => repository.GetById(new ProductId(id)), Times.Once);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-12345)]
    public void GetProductById_ThrowProductDoesNotExistException(long id)
    {
        // Arrange
        var repositoryMock = new Mock<IProductRepository>();
        repositoryMock.Setup(repository =>
                repository.GetById(It.Is<ProductId>(id => id.Id <= 0)))
            .Throws(RepositoryException.ProductDoesNotExists);
        
        var service = new ProductService(repositoryMock.Object, _mapper);
        
        // Assert
        Assert.Throws<RepositoryException>(() => service.GetProductById(id));
        
        repositoryMock.Verify(repository => repository.GetById(new ProductId(id)), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ProductDtoData))]
    public void CreateProduct(ProductDto productDto)
    {
        // Arrange
        var repositoryMock = new Mock<IProductRepository>();
        repositoryMock.Setup(repository => repository.Insert(It.IsAny<Product>()))
            .Returns<Product>(product => product);
        
        var service = new ProductService(repositoryMock.Object, _mapper);
        
        // Act
        service.CreateProduct(productDto);

        // Assert
        repositoryMock.Verify(repository =>
            repository.Insert(_mapper.Map<Product>(productDto)), Times.Once);
    }
    
    [Theory]
    [MemberData(nameof(ProductDtoExceptionData))]
    public void CreateProduct_ThrowProductAlreadyExistsException(ProductDto productDto, ProductDto otherProductDto)
    {
        // Arrange
        var methodCalls = 0;
        
        var repositoryMock = new Mock<IProductRepository>();
        repositoryMock.Setup(repository => repository.Insert(It.IsAny<Product>()))
            .Returns<Product>(product =>
            {
                if (methodCalls >= 1)
                {
                    throw RepositoryException.ProductAlreadyExists(product.Id);
                }

                methodCalls++;
                return product;
            });
        
        var service = new ProductService(repositoryMock.Object, _mapper);
        
        // Act
        service.CreateProduct(productDto);
        
        // Assert
        Assert.Throws<RepositoryException>(() => service.CreateProduct(productDto));

        repositoryMock.Verify(repository =>
            repository.Insert(_mapper.Map<Product>(productDto)), Times.Exactly(2));
    }
    
    public static IEnumerable<object[]> ProductDtoData()
    {
        yield return new object[]
        {
            new ProductDto(
                Id: 1,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12,
                Category: "Chemicals",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now),
                WarehouseId: 124)
        };
        yield return new object[]
        {
            new ProductDto(
                Id: 142,
                Name: "Krutoi Bober",
                Price: 154.6m,
                Weight: 12,
                Category: "Chemicals",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now),
                WarehouseId: 124)
        };
        yield return new object[]
        {
            new ProductDto(
                Id: 121,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12,
                Category: "Chemicals",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now),
                WarehouseId: 124)
        };
    }

    public static IEnumerable<object[]> ProductDtoExceptionData()
    {
        yield return new object[]
        {
            new ProductDto(
                Id: 1,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12,
                Category: "Chemicals",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now),
                WarehouseId: 124),
            new ProductDto(
                Id: 1,
                Name: "Ne krutoi :(",
                Price: 15.6333m,
                Weight: 11322,
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now),
                WarehouseId: 1256)
        };
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    [InlineData(12345)]
    public void DeleteProductById(long id)
    {
        // Arrange
        var repositoryMock = new Mock<IProductRepository>();
        repositoryMock.Setup(repository => repository.GetById(It.IsAny<ProductId>()))
            .Returns<ProductId>(productId =>
                InitializeProduct(
                    productId.Id,
                    "SomeName",
                    123.2m,
                    1.42,
                    ProductCategory.Chemicals,
                    DateOnly.MinValue,
                    234));
        
        repositoryMock.Setup(repository => repository.DeleteById(It.IsAny<ProductId>()));

        var service = new ProductService(repositoryMock.Object, _mapper);

        // Act
        service.DeleteProductById(id);
        
        // Assert
        repositoryMock.Verify(repository => repository.DeleteById(new ProductId(id)), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    [InlineData(12345)]
    public void DeleteProductById_ThrowProductDoesNotExistException(long id)
    {
        // Arrange
        var repositoryMock = new Mock<IProductRepository>();
        repositoryMock.Setup(repository => repository.DeleteById(It.IsAny<ProductId>()))
            .Throws(RepositoryException.ProductDoesNotExists);;

        var service = new ProductService(repositoryMock.Object, _mapper);

        // Assert
        Assert.Throws<RepositoryException>(() => service.DeleteProductById(id));
        
        repositoryMock.Verify(repository => repository.DeleteById(new ProductId(id)), Times.Once);
    }

    [Theory]
    [InlineData(1, 1234)]
    [InlineData(2, 1234.4)]
    [InlineData(215, 2334.234)]
    public void UpdateProductPrice(long id, decimal price)
    {
        // Arrange
        var repositoryMock = new Mock<IProductRepository>();
        repositoryMock.Setup(repository => repository.GetById(It.IsAny<ProductId>()))
            .Returns<ProductId>(productId =>
                InitializeProduct(
                    productId.Id,
                    "SomeName",
                    123.2m,
                    1.42,
                    ProductCategory.Chemicals,
                    DateOnly.MinValue,
                    234));
        repositoryMock.Setup(repository => repository.Update(It.IsAny<Product>()))
            .Returns<Product>(product => product);
        
        var service = new ProductService(repositoryMock.Object, _mapper);

        // Act
        service.UpdateProductPrice(id, price);
        
        // Assert
        repositoryMock.Verify(repository => repository.GetById(new ProductId(id)), Times.Once);
        
        repositoryMock.Verify(repository =>
            repository.Update(InitializeProduct(
                id,
                "SomeName",
                price,
                1.42,
                ProductCategory.Chemicals,
                DateOnly.MinValue,
                234)), 
            Times.Once);
    }

    [Theory]
    [InlineData(1, 1234)]
    public void UpdateProductPrice_ThrowProductDoesNotExistException(long id, decimal price)
    {
        // Arrange
        var repositoryMock = new Mock<IProductRepository>();
        repositoryMock.Setup(repository => repository.GetById(It.IsAny<ProductId>()))
            .Throws(RepositoryException.ProductDoesNotExists);
        repositoryMock.Setup(repository => repository.Update(It.IsAny<Product>()))
            .Returns<Product>(product => product);
        
        var service = new ProductService(repositoryMock.Object, _mapper);

        // Assert
        Assert.Throws<RepositoryException>(() => service.UpdateProductPrice(id, price));
        
        repositoryMock.Verify(repository => repository.GetById(new ProductId(id)), Times.Once);
        
        repositoryMock.Verify(repository =>
            repository.Update(InitializeProduct(
                id,
                "SomeName",
                price,
                1.42,
                ProductCategory.Chemicals,
                DateOnly.MinValue,
                234)), 
            Times.Never);
    }

    [Theory]
    [MemberData(nameof(FiltersDtoData))]
    public void GetProductsFiltered(FiltersDto filtersDto)
    {
        // Arrange
        var repositoryMock = new Mock<IProductRepository>();
        repositoryMock.Setup(repository => repository.GetAll())
            .Returns(new Dictionary<ProductId, Product>()
            {
                { new ProductId(1),
                    InitializeProduct(
                        1,
                        "Goofy aah",
                        123.2m,
                        1.42,
                        ProductCategory.Chemicals,
                        DateOnly.MinValue,
                        234) },
                { new ProductId(2),
                    InitializeProduct(
                        2,
                        "Sad oh",
                        123.2m,
                        1.42,
                        ProductCategory.Food,
                        DateOnly.MinValue,
                        234) },
                { new ProductId(3),
                    InitializeProduct(
                        3,
                        "Monkey sheesh",
                        123.2m,
                        1.42,
                        ProductCategory.Chemicals,
                        DateOnly.MinValue,
                        234) },
            });

        var service = new ProductService(repositoryMock.Object, _mapper);

        // Act
        service.GetProductsFiltered(filtersDto);
        
        // Assert
        repositoryMock.Verify(repository => repository.GetAll(), Times.Once);
    }

    public static IEnumerable<object[]> FiltersDtoData()
    {
        yield return new object[]
        {
            new FiltersDto()
            {
                ProductManufactureDateFilter = DateOnly.FromDateTime(DateTime.Now)
            }
        };
        yield return new object[]
        {
            new FiltersDto()
            {
                ProductManufactureDateFilter = DateOnly.FromDateTime(DateTime.Now),
                ProductCategoryFilter = "Food",
                ProductWarehouseIdFilter = 123
            }
        };
        yield return new object[]
        {
            new FiltersDto()
            {
                ProductWarehouseIdFilter = 123
            }
        };
        yield return new object[]
        {
            new FiltersDto()
            {
                ProductCategoryFilter = "Food",
            }
        };
    }
    
    private Product InitializeProduct(
        long productId,
        string productName,
        decimal productPrice,
        double productWeight,
        ProductCategory productCategory,
        DateOnly productManufactureDate,
        long warehouseId) 
    {
        var product = new Product(
            new ProductId(productId),
            new ProductName(productName),
            new ProductPrice(productPrice),
            new ProductWeight(productWeight),
            productCategory,
            productManufactureDate,
            new WarehouseId(warehouseId));

        return product;
    }
}