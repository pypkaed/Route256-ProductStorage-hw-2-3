using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductGrpc;
using Products;
using ProductsPresentationTest.Helpers;
using ProductsPresentationTest.Json;
using Xunit;

namespace ProductsPresentationTest;

public class PresentationRestIntegrationTest : IClassFixture<MockServiceWebApplicationFactory<Program>>
{
    private readonly MockServiceWebApplicationFactory<Program> _factory;

    public PresentationRestIntegrationTest()
    {
        _factory = new MockServiceWebApplicationFactory<Program>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(145657)]
    [InlineData(131245678)]
    public async Task RestGetProductByIdTest(long id)
    {
        var webAppClient = _factory.CreateClient();

        var requestModel = new GetProductByIdRequest()
        {
            Id = id
        };

        var jsonRequest = JsonSerializer.Serialize(requestModel).ToLower();
        var request = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await webAppClient.PostAsync("/v1/products/get-product-by-id", request);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseProduct = JsonSerializer.Deserialize<ProductResponseJson>(responseContent, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        
        responseProduct.Should().NotBeNull();
        responseProduct.Id.Should().Be(id);
    }
    
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-123)]
    public async Task RestGetProductById_ThrowsValidationException_Test(long id)
    {
        var webAppClient = _factory.CreateClient();

        var requestModel = new GetProductByIdRequest() 
        {
            Id = id
        };
        
        var jsonRequest = JsonSerializer.Serialize(requestModel).ToLower();
        var request = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        
        var response = await webAppClient.PostAsync("/v1/products/get-product-by-id", request);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Theory]
    [MemberData(nameof(CreateProductRequestJsonData))]
    public async Task RestCreateProductTest(CreateProductRequestJson requestModel)
    {
        var factory = new WebApplicationFactory<Program>();
        var webAppClient = factory.CreateClient();
        
        var jsonRequest = JsonSerializer.Serialize(requestModel).ToLower();
        var request = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
    
        var response = await webAppClient.PostAsync("/v1/products/create-product", request);
        
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseProduct = JsonSerializer.Deserialize<ProductResponseJson>(responseContent, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        
        responseProduct.Should().NotBeNull();
        responseProduct.Id.Should().Be((int)requestModel.Id);
    }
    
    public static IEnumerable<object[]> CreateProductRequestJsonData()
    {
        yield return new object[]
        {
            new CreateProductRequestJson()
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
            new CreateProductRequestJson()
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
            new CreateProductRequestJson()
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
    [MemberData(nameof(CreateProductRequestJsonExceptionData))]
    public async Task RestCreateProduct_ThrowValidationException_Test(CreateProductRequestJson requestModel)
    {
        var webAppClient = _factory.CreateClient();
        
        var jsonRequest = JsonSerializer.Serialize(requestModel).ToLower();
        var request = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        
        var response = await webAppClient.PostAsync("/v1/products/create-product", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
    public static IEnumerable<object[]> CreateProductRequestJsonExceptionData()
    {
        yield return new object[]
        {
            new CreateProductRequestJson()
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
            new CreateProductRequestJson()
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
            new CreateProductRequestJson()
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
}