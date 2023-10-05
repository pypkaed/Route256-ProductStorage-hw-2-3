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
}