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
            services.RemoveAll<IProductService>();

            services.Replace(ServiceDescriptor.Singleton<IProductService>(_ =>
            {
                var serviceMock = new Mock<IProductService>();

                var product = new ProductDto(
                    Id: 1,
                    Name: "Krutoi Bober",
                    Price: 15.6m,
                    Weight: 12,
                    Category: "Chemicals",
                    ManufactureDate: DateOnly.FromDateTime(DateTime.Now),
                    WarehouseId: 124);
                
                serviceMock
                    .Setup(service => service.GetProductById(product.Id))
                    .Returns(product);
        
                return serviceMock.Object;
            }));
        });
    }
}