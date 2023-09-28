using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductsDao.Repositories;
using ProductsDao.Validators.Entities;

namespace ProductsDao.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDao(
        this IServiceCollection collection)
    {
        collection.AddScoped<IProductRepository, ProductInMemoryRepository>();
        collection.AddValidatorsFromAssembly(typeof(ProductValidator).Assembly);

        return collection;
    }
}