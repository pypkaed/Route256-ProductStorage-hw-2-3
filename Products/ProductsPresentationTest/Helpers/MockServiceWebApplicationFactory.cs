using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using ProductsBusiness.Dto;
using ProductsBusiness.Services;

namespace ProductsPresentationTest.Helpers;

public class MockServiceWebApplicationFactory<TProgram> 
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(services =>
        {
            services.Replace(ServiceDescriptor.Singleton<IProductService>(_ =>
            {
                var serviceMock = new Mock<IProductService>();
                
                serviceMock
                    .Setup(service => service.GetProductById(It.IsAny<long>()))
                    .Returns<long>((long id) => new ProductDto(
                        Id: id,
                        Name: "Krutoi Bober",
                        Price: 15.6m,
                        Weight: 12,
                        Category: "Chemicals",
                        ManufactureDate: DateOnly.FromDateTime(DateTime.Now),
                        WarehouseId: 124));

                serviceMock
                    .Setup(service => service.CreateProduct(It.IsAny<ProductDto>()))
                    .Returns<ProductDto>((ProductDto request) => new ProductDto
                    (request.Id,
                    request.Name,
                    request.Price,
                    request.Weight,
                    request.Category,
                    request.ManufactureDate,
                    request.WarehouseId));
                
                serviceMock.Verify(x => 
                    x.CreateProduct(It.IsAny<ProductDto>()),
                    Times.AtMostOnce);
                
                return serviceMock.Object;
            }));
        });
    }
}