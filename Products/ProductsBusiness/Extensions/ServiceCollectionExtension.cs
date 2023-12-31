using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductsBusiness.Services;

namespace ProductsBusiness.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBusiness(
        this IServiceCollection collection)
    {
        collection.AddScoped<IProductService, ProductService>();
        collection.AddAutoMapper(typeof(IProductService));

        return collection;
    }
}