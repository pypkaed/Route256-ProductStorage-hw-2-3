using AutoMapper;
using ProductsBusiness.Dto;
using ProductsBusiness.Profiles;
using ProductsBusiness.Services;
using ProductsDao.Entities;
using ProductsDao.Exceptions;
using ProductsDao.Models;
using ProductsDao.Repositories;
using Xunit;
using MockRepository = ProductsBusinessTest.Helpers.MockRepository;

namespace ProductsBusinessTest;

public class ProductsServiceTest
{
    private readonly IProductService _service;

    public ProductsServiceTest()
    {
        var mapperConfiguration = new MapperConfiguration(expression =>
        {
            expression.AddProfile<ProductProfile>();
        });

        var mapper = new Mapper(mapperConfiguration);
        var repository = new ProductInMemoryRepository();
        
        _service = new ProductService(repository, mapper);
    }

    [Fact]
    public void CreateProductTest()
    {
        var productDto = new ProductDto(
            Id: 1,
            Name: "Krutoi Bober",
            Price: 123m,
            Weight: 14, 
            Category: "Food",
            ManufactureDate: DateOnly.MaxValue, 
            WarehouseId: 15);
        _service.CreateProduct(productDto);

        Assert.Equal(productDto, _service.GetProductById(productDto.Id));
    } 
    
    [Fact]
    public void CreateSameProduct_ThrowRepeatingProductException_Test()
    {
        var productDto = new ProductDto(
            Id: 1,
            Name: "Krutoi Bober",
            Price: 123m,
            Weight: 14, 
            Category: "Food",
            ManufactureDate: DateOnly.MaxValue, 
            WarehouseId: 15);
        _service.CreateProduct(productDto);

        Assert.Throws<RepositoryException>(() =>
        {
            _service.CreateProduct(productDto);
        });
    } 
    
    [Theory]
    [MemberData(nameof(ProductDtoExceptionData))]
    public void CreateProductWithSameId_ThrowRepeatingProductException_Test(
        ProductDto productDto,
        ProductDto otherProductDto)
    {
        _service.CreateProduct(productDto);

        Assert.Throws<RepositoryException>(() =>
        {
            _service.CreateProduct(otherProductDto);
        });
    }

    public static IEnumerable<object[]> ProductDtoExceptionData()
    {
        yield return new object[]
        {
            new ProductDto(
                Id: 1,
                Name: "Krutoi Bober",
                Price: 123m,
                Weight: 14,
                Category: "Food",
                ManufactureDate: DateOnly.MaxValue,
                WarehouseId: 15),
            new ProductDto(
                Id: 1,
                Name: "Lame Bober",
                Price: 13m,
                Weight: 4,
                Category: "Electronics",
                ManufactureDate: DateOnly.MinValue,
                WarehouseId: 1)
        };
    }

    [Fact]
    public void GetProductByIdTest()
    {
        var productId = 1;
        
        var product = new Product(
            new ProductId(productId),
            new ProductName("Krutoi Bober"),
            new ProductPrice(15.6m),
            new ProductWeight(12), 
            ProductCategory.Chemicals,
            DateOnly.FromDateTime(DateTime.Now),
            new WarehouseId(124));

        var service = MockRepository.InitializeServiceWithMockRepositoryGetById(product);
        
        var productDto = new ProductDto(
            Id: productId,
            Name: "Krutoi Bober",
            Price: 15.6m,
            Weight: 12, 
            Category: "Chemicals",
            ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
            WarehouseId: 124);

        Assert.Equal(productDto, service.GetProductById(productDto.Id));
    }
    
    [Fact]
    public void GetProductById_ThrowProductNotFoundException_Test()
    {
        Assert.Throws<RepositoryException>(() =>
        {
            _service.GetProductById(1);
        });
    }

    [Theory]
    [MemberData(nameof(ProductDtoData))]
    public void DeleteProductByIdTest(ProductDto productDto)
    {
        _service.CreateProduct(productDto);
        
        _service.DeleteProductById(productDto.Id);

        Assert.Throws<RepositoryException>(() =>
        {
            _service.GetProductById(productDto.Id);
        });
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
    
    [Fact]
    public void DeleteProductById_ThrowProductNotFoundException_Test()
    {
        Assert.Throws<RepositoryException>(() =>
        {
            _service.GetProductById(1);
        });
    }

    [Theory]
    [MemberData(nameof(ProductDtoData))]
    public void UpdateProductPriceTest(ProductDto productDto)
    {
        _service.CreateProduct(productDto);

        var newPrice = 1434.3m;
        _service.UpdateProductPrice(productDto.Id, newPrice);

        var resultPrice = _service.GetProductById(productDto.Id).Price;
        Assert.Equal(newPrice, resultPrice);
    }
    
    [Fact]
    public void UpdateProductPrice_ThrowProductNotFoundException_Test()
    {
        Assert.Throws<RepositoryException>(() =>
        {
            _service.UpdateProductPrice(1, 123m);
        });
    }

    [Fact]
    public void GetProductsFiltered_AllFilters_Test()
    {
        var filters = new FiltersDto()
        {
            ProductManufactureDateFilter = DateOnly.FromDateTime(DateTime.Now),
            ProductCategoryFilter = "Food",
            ProductWarehouseIdFilter = 123,
        };

        var products = new List<ProductDto>();
        for (long productId = 1; productId < 50; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            products.Add(productDto);
            _service.CreateProduct(productDto);
        }
        for (long productId = 50; productId < 100; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Chemicals",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            _service.CreateProduct(productDto);
        }

        var result = _service.GetProductsFiltered(filters);
        Assert.Equal(products, result);
    }

    [Fact]
    public void GetProductsFiltered_GetPage_AllFilters_Test()
    {
        var filters = new FiltersDto()
        {
            ProductManufactureDateFilter = DateOnly.FromDateTime(DateTime.Now),
            ProductCategoryFilter = "Food",
            ProductWarehouseIdFilter = 123,
        };

        var products = new List<ProductDto>();
        for (long productId = 0; productId < 50; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            products.Add(productDto);
            _service.CreateProduct(productDto);
        }
        for (long productId = 50; productId < 100; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Chemicals",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            _service.CreateProduct(productDto);
        }

        var filteredResult = _service.GetProductsFiltered(filters);
        Assert.Equal(products.Take(3), _service.GetPage(1, 3, filteredResult));
    }
    
    [Fact]
    public void GetProductsFiltered_ProductManufactureDateFilter_AllProductsResult_Test()
    {
        var filters = new FiltersDto()
        {
            ProductManufactureDateFilter = DateOnly.FromDateTime(DateTime.Now)
        };

        var products = new List<ProductDto>();
        for (long productId = 1; productId < 50; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            products.Add(productDto);
            _service.CreateProduct(productDto);
        }
        for (long productId = 50; productId < 100; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now.AddDays(13)), 
                WarehouseId: 123);
            
            _service.CreateProduct(productDto);
        }

        var result = _service.GetProductsFiltered(filters);
        Assert.Equal(products, result);
    }

    [Fact]
    public void GetProductsFiltered_GetPage_ProductManufactureDateFilter_AllProductsResult_Test()
    {
        var filters = new FiltersDto()
        {
            ProductManufactureDateFilter = DateOnly.FromDateTime(DateTime.Now)
        };

        var products = new List<ProductDto>();
        for (long productId = 1; productId < 50; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            products.Add(productDto);
            _service.CreateProduct(productDto);
        }
        for (long productId = 50; productId < 100; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now.AddDays(13)), 
                WarehouseId: 123);
            
            _service.CreateProduct(productDto);
        }

        var filteredResult = _service.GetProductsFiltered(filters);
        var result = _service.GetPage(1, 3, filteredResult);

        Assert.Equal(products.Take(3), result);
    }
    [Fact]
    public void GetProductsFiltered_ProductCategoryFilter_AllProductsResult_Test()
    {
        var filters = new FiltersDto()
        {
            ProductCategoryFilter = "Food"
        };

        var products = new List<ProductDto>();
        for (long productId = 1; productId < 50; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            products.Add(productDto);
            _service.CreateProduct(productDto);
        }
        for (long productId = 50; productId < 100; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Chemicals",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            _service.CreateProduct(productDto);
        }

        var result = _service.GetProductsFiltered(filters);
        Assert.Equal(products, result);
    }

    [Fact]
    public void GetProductsFiltered_GetPage_AllFilters_AllProductsResult_Test()
    {
        var filters = new FiltersDto()
        {
            ProductCategoryFilter = "Food"
        };

        var products = new List<ProductDto>();
        for (long productId = 1; productId < 50; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            products.Add(productDto);
            _service.CreateProduct(productDto);
        }
        for (long productId = 50; productId < 100; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Chemicals",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            _service.CreateProduct(productDto);
        }

        var filteredResult = _service.GetProductsFiltered(filters);
        var result = _service.GetPage(1, 3, filteredResult);
        Assert.Equal(products.Take(3), result);
    }
    
    [Fact]
    public void GetProductsFiltered_ProductWarehouseIdFilter_AllProductsResult_Test()
    {
        var filters = new FiltersDto()
        {
            ProductWarehouseIdFilter = 123
        };
        
        var products = new List<ProductDto>();
        for (long productId = 1; productId < 50; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
            
            products.Add(productDto);
            _service.CreateProduct(productDto);
        }
        for (long productId = 50; productId < 100; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 12345);
            
            _service.CreateProduct(productDto);
        }

        
        Assert.Equal(products, _service.GetProductsFiltered(filters));
    }
    
    [Fact]
    public void GetProductsFiltered_GetPage_ProductWarehouseIdFilter_AllProductsResult_Test()
    {
        var filters = new FiltersDto()
        {
            ProductWarehouseIdFilter = 123
        };
            
        var products = new List<ProductDto>();
        for (long productId = 1; productId < 50; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 123);
                    
            products.Add(productDto);
            _service.CreateProduct(productDto);
        }
        for (long productId = 50; productId < 100; productId++)
        {
            var productDto = new ProductDto(
                Id: productId,
                Name: "Krutoi Bober",
                Price: 15.6m,
                Weight: 12, 
                Category: "Food",
                ManufactureDate: DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId: 12345);
                    
            _service.CreateProduct(productDto);
        }
    
        var filteredResult = _service.GetProductsFiltered(filters);
        var result = _service.GetPage(1, 3, filteredResult);
        Assert.Equal(products.Take(3), result);
    }
}