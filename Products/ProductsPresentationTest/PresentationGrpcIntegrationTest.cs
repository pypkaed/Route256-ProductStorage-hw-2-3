using FluentAssertions;
using Grpc.Net.Client;
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
    
    // rpc CreateProduct (CreateProductRequest) returns (ProductResponse) {
    //     option (google.api.http) = {
    //         post: "/v1/products/create-product",
    //         body: "*"
    //     };
    // }

    // rpc DeleteProductById (DeleteProductByIdRequest) returns (google.protobuf.Empty) {
    //     option (google.api.http) = {
    //         delete: "/v1/products/delete-product-by-id",
    //         body: "*"
    //     };
    // }
    // rpc UpdateProductPrice (UpdateProductPriceRequest) returns (ProductResponse) {
    //     option (google.api.http) = {
    //         put: "/v1/products/update-product-price",
    //         body: "*"
    //     };
    // }
    // rpc GetProductsFiltered(GetProductsFilteredRequest) returns (GetProductsFilteredResponse) {
    //     option (google.api.http) = {
    //         post: "/v1/products/get-products-filtered",
    //         body: "*"
    //     };
    // }
}