using System.Text;
using System.Text.Json;
using FluentAssertions;
using ProductGrpc;
using Products;
using ProductsBusiness.Dto;
using ProductsPresentationTest.Helpers;
using Xunit;

namespace ProductsPresentationTest;

public class PresentationRestIntegrationTest : IClassFixture<MockServiceWebApplicationFactory<Program>>
{
    private readonly MockServiceWebApplicationFactory<Program> _factory;

    public PresentationRestIntegrationTest()
    {
        _factory = new MockServiceWebApplicationFactory<Program>();
    }

    [Fact]
    public async Task RestGetProductByIdTest()
    {
        var webAppClient = _factory.CreateClient();

        var requestModel = new GetProductByIdRequest()
        {
            Id = 1
        };

        var jsonRequest = JsonSerializer.Serialize(requestModel);
        var request = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await webAppClient.PostAsync("/v1/products/get-product-by-id", request);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseProduct = JsonSerializer.Deserialize<ProductDto>(responseContent);
        
        responseProduct.Should().NotBeNull();
        responseProduct.Id.Should().Be(1);
    }
}