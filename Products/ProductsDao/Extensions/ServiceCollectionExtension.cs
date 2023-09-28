using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductsDao.Repositories;

namespace ProductsDao.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDao(
        this IServiceCollection collection)
    {
        collection.AddSingleton<IProductRepository, ProductInMemoryRepository>();

        return collection;
    }
}