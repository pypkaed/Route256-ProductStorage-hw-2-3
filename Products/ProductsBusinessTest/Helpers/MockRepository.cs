using AutoMapper;
using Moq;
using ProductsBusiness.Profiles;
using ProductsBusiness.Services;
using ProductsDao.Entities;
using ProductsDao.Repositories;

namespace ProductsBusinessTest.Helpers;

public static class MockRepository
{
    public static IProductService InitializeServiceWithMockRepositoryGetById(Product product)
    {
        var repositoryMock = new Mock<IProductRepository>();
        repositoryMock
            .Setup(repository => repository.GetById(product.Id))
            .Returns(product);
        
        var repository = repositoryMock.Object;
        var mapperConfiguration = new MapperConfiguration(expression =>
        {
            expression.AddProfile<ProductProfile>();
        });

        var mapper = new Mapper(mapperConfiguration);

        var service = new ProductService(repository, mapper);

        return service;
    }
}