using FluentAssertions;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductGrpc;
using Products;
using ProductsPresentationTest.Helpers;
using Xunit;

namespace ProductsPresentationTest;

public class PresentationGrpcIntegrationTest : IClassFixture<MockServiceWebApplicationFactory<Program>>
{
    private readonly MockServiceWebApplicationFactory<Program> _factory;

    public PresentationGrpcIntegrationTest()
    {
        _factory = new MockServiceWebApplicationFactory<Program>();
    }
    

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(99999999)]
    public void GrpcGetProductById_Success(long id)
    {
        // Arrange
        var webAppClient = _factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });
        
        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);
        
        var request = new GetProductByIdRequest() 
        {
            Id = id
        };
        
        // Act
        var response = grpcClient.GetProductById(request);
        var expected = response.Id;
        var actual = request.Id;

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-123)]
    public void GrpcGetProductById_ThrowsValidationException(long id)
    {
        // Arrange
        var webAppClient = _factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);

        var request = new GetProductByIdRequest() 
        {
            Id = id
        };

        // Assert
        Assert.Throws<RpcException>(() =>
        {
            grpcClient.GetProductById(request);
        });
    }

    [Theory]
    [MemberData(nameof(CreateProductRequestData))]
    public void GrpcCreateProduct_Success(CreateProductRequest request)
    {
        // Arrange
        var webAppClient = _factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);

        // Act
        var response = grpcClient.CreateProduct(request);
        var expected = response.Id;
        var actual = request.Id;
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> CreateProductRequestData()
    {
        yield return new object[]
        {
            new CreateProductRequest()
            {
                Id = 23,
                Name = "Bebrijon",
                Price = new ProductGrpc.Decimal()
                {
                    Units = 123,
                    Nanos = 10000000,
                },
                Weight = 3245.4,
                Category = ProductCategory.Electronics,
                ManufactureDate = "2022/01/01",
                WarehouseId = 12345
            }
        };
        yield return new object[]
        {
            new CreateProductRequest()
            {
                Id = 1,
                Name = "Bebrijon",
                Price = new ProductGrpc.Decimal()
                {
                    Units = 143,
                    Nanos = 1940,
                },
                Weight = 3245.4,
                Category = ProductCategory.Electronics,
                ManufactureDate = "2023/01/01",
                WarehouseId = 12345
            }
        };
        yield return new object[]
        {
            new CreateProductRequest()
            {
                Id = 67,
                Name = "a",
                Price = new ProductGrpc.Decimal()
                {
                    Units = 123,
                    Nanos = 10000000,
                },
                Weight = 3245.4,
                Category = ProductCategory.Electronics,
                ManufactureDate = "2022/01/01",
                WarehouseId = 12345
            }
        };
    }

    
    [Theory]
    [MemberData(nameof(CreateProductRequestExceptionData))]
    public void GrpcCreateProduct_ThrowValidationException(CreateProductRequest request)
    {
        // Arrange
        var webAppClient = _factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);

        // Assert
        Assert.Throws<RpcException>(() => grpcClient.CreateProduct(request));
    }
    
    public static IEnumerable<object[]> CreateProductRequestExceptionData()
    {
        yield return new object[]
        {
            new CreateProductRequest()
            {
                Id = -123,
                Name = "Bebrijon",
                Price = new ProductGrpc.Decimal()
                {
                    Units = 123,
                    Nanos = 10000000,
                },
                Weight = 3245.4,
                Category = ProductCategory.Electronics,
                ManufactureDate = "2022/01/01",
                WarehouseId = 12345
            }
        };
        yield return new object[]
        {
            new CreateProductRequest()
            {
                Id = 1,
                Name = "",
                Price = new ProductGrpc.Decimal()
                {
                    Units = 143,
                    Nanos = 1940,
                },
                Weight = 3245.4,
                Category = ProductCategory.Electronics,
                ManufactureDate = "2023/01/01",
                WarehouseId = 12345
            }
        };
        yield return new object[]
        {
            new CreateProductRequest()
            {
                Id = 67,
                Name = "a",
                Price = new ProductGrpc.Decimal()
                {
                    Units = 123,
                    Nanos = 10000000,
                },
                Weight = 3245.4,
                Category = ProductCategory.Electronics,
                ManufactureDate = "202234/01/01",
                WarehouseId = 12345
            }
        };
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    [InlineData(1234566)]
    public void DeleteProductById_Success(long id)
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var webAppClient = factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);

        var createProductRequest = new CreateProductRequest()
        {
            Id = id,
            Name = "Bebrijon",
            Price = new ProductGrpc.Decimal()
            {
                Units = 123,
                Nanos = 10000000,
            },
            Weight = 3245.4,
            Category = ProductCategory.Electronics,
            ManufactureDate = "2022/01/01",
            WarehouseId = 12345
        };
        
        grpcClient.CreateProduct(createProductRequest);
        
        var request = new DeleteProductByIdRequest()
        {
            Id = id
        };

        // Act
        grpcClient.DeleteProductById(request);
        
        // Assert
        Assert.Throws<RpcException>(() => grpcClient.GetProductById(new GetProductByIdRequest { Id = id }));
    }

    [Theory]
    [MemberData(nameof(IdAndProductGrpcDecimalData))]
    public void UpdateProductPrice_Success(long id, ProductGrpc.Decimal actual)
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var webAppClient = factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);

        var createProductRequest = new CreateProductRequest()
        {
            Id = id,
            Name = "Bebrijon",
            Price = new ProductGrpc.Decimal()
            {
                Units = 123,
                Nanos = 10000000,
            },
            Weight = 3245.4,
            Category = ProductCategory.Electronics,
            ManufactureDate = "2022/01/01",
            WarehouseId = 12345
        };
        
        grpcClient.CreateProduct(createProductRequest);
        
        var request = new UpdateProductPriceRequest()
        {
            Id = id,
            Price = actual
        };

        // Act
        var response = grpcClient.UpdateProductPrice(request);
        var expected = response.Price;

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> IdAndProductGrpcDecimalData()
    {
        yield return new object[]
        {
            123,
            new ProductGrpc.Decimal()
            {
                Units = 132,
                Nanos = 234567892,
            }
        };
    }
    
    [Fact]
    public void GetProductsFiltered_Success()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();

        var webAppClient = factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);
        
        var actual = new List<ProductResponse>();
        
        // Act
        for (int productId = 1; productId < 50; productId++)
        {
            var createProductRequest = new CreateProductRequest()
            {
                Id = productId,
                Name = "Bebrijon",
                Price = new ProductGrpc.Decimal()
                {
                    Units = 123,
                    Nanos = 10000000,
                },
                Weight = 3245.4,
                Category = ProductCategory.Electronics,
                ManufactureDate = "2022/01/01",
                WarehouseId = 12345
            };

            var productResponse = grpcClient.CreateProduct(createProductRequest);
            actual.Add(productResponse);
        }

        var request = new GetProductsFilteredRequest
        {
            ProductCategoryFilter = "Electronics"
        };

        // Assert
        var response = grpcClient.GetProductsFiltered(request);
        var expected = response.Products;
        
        Assert.NotNull(response);
        Assert.Equal(expected, actual);
    }
}