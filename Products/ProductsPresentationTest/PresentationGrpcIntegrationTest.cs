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
    
    // TODO: add exception tests.......

    [Fact]
    public void GrpcGetProductByIdTest()
    {
        var webAppClient = _factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);

        var request = new GetProductByIdRequest() 
        {
            Id = 1
        };
        
        var response = grpcClient.GetProductById(request);
        response.Should().NotBeNull();
        response.Id.Should().Be(1);
    }

    [Fact]
    public void GrpcCreateProductTest()
    {
        var webAppClient = _factory.CreateClient();
        var channel = GrpcChannel.ForAddress(webAppClient.BaseAddress, new GrpcChannelOptions()
        {
            HttpClient = webAppClient
        });

        var grpcClient = new ProductGrpcService.ProductGrpcServiceClient(channel);

        var request = new CreateProductRequest()
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
        };

        var response = grpcClient.CreateProduct(request);
        
        response.Should().NotBeNull();
        response.Id.Should().Be(request.Id);
    }

    [Fact]
    public void DeleteProductByIdTest()
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
            Id = 1,
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
            Id = 1
        };

        grpcClient.DeleteProductById(request);
        Assert.Throws<RpcException>(() => grpcClient.GetProductById(new GetProductByIdRequest { Id = request.Id }));
    }

    [Fact]
    public void UpdateProductPriceTest()
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
            Id = 1,
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

        var newPrice = new ProductGrpc.Decimal()
        {
            Units = 1234,
            Nanos = 200000000,
        };
        
        var request = new UpdateProductPriceRequest()
        {
            Id = 1,
            Price = newPrice
        };

        var response = grpcClient.UpdateProductPrice(request);

        response.Should().NotBeNull();
        response.Price.Should().Be(newPrice);
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