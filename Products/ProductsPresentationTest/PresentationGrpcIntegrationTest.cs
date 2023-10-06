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
    public void GrpcGetProductByIdTest(long id)
    {
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
        
        var response = grpcClient.GetProductById(request);
        response.Should().NotBeNull();
        response.Id.Should().Be(id);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-123)]
    public void GrpcGetProductById_ThrowsValidationException_Test(long id)
    {
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

        Assert.Throws<RpcException>(() =>
        {
            grpcClient.GetProductById(request);
        });
    }

    [Theory]
    [MemberData(nameof(CreateProductRequestData))]
    public void GrpcCreateProductTest(CreateProductRequest request)
    {
        var webAppClient = _factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);

        var response = grpcClient.CreateProduct(request);
        
        response.Should().NotBeNull();
        response.Id.Should().Be(request.Id);
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
    public void GrpcCreateProduct_ThrowValidationException_Test(CreateProductRequest request)
    {
        var webAppClient = _factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);

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
    public void DeleteProductByIdTest(long id)
    {
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

        grpcClient.DeleteProductById(request);
        Assert.Throws<RpcException>(() => grpcClient.GetProductById(new GetProductByIdRequest { Id = id }));
    }

    [Theory]
    [MemberData(nameof(IdAndProductGrpcDecimalData))]
    public void UpdateProductPriceTest(long id, ProductGrpc.Decimal newPrice)
    {
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
            Price = newPrice
        };

        var response = grpcClient.UpdateProductPrice(request);

        response.Should().NotBeNull();
        response.Price.Should().Be(newPrice);
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
    public void GetProductsFilteredTest()
    {
        var factory = new WebApplicationFactory<Program>();

        var webAppClient = factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);
        
        var products = new List<ProductResponse>();
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
            products.Add(productResponse);
        }

        var request = new GetProductsFilteredRequest
        {
            ProductCategoryFilter = "Electronics"
        };

        var response = grpcClient.GetProductsFiltered(request);

        response.Should().NotBeNull();
        response.Products.Should().Equal(products);
    }
}