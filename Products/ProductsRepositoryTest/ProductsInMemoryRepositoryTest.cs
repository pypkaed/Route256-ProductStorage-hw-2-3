using ProductsDao.Entities;
using ProductsDao.Exceptions;
using ProductsDao.Models;
using ProductsDao.Repositories;
using Xunit;

namespace ProductsTest;

public class ProductsInMemoryRepositoryTest
{
    private readonly IProductRepository _repository;

    public ProductsInMemoryRepositoryTest()
    {
        _repository = new ProductInMemoryRepository();
    }

    [Fact]
    public void GetByIdTest()
    {
        var productId = 1;
        
        var product = CreateProduct(
            productId: productId,
            productName: "Bebra",
            productPrice: 15.6m,
            productWeight: 12,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);
        
        _repository.Insert(product);
        
        Assert.Equal(product, _repository.GetById(new ProductId(productId)));
    }

    [Fact]
    public void GetById_ThrowProductDoesNotExistRepositoryException_Test()
    {
        Assert.Throws<RepositoryException>(() =>
        {
            _repository.GetById(new ProductId(1));
        });
    }

    [Fact]
    public void GetAllTest()
    {
        var products = new List<Product>();
        for (long productId = 1; productId < 100; productId++)
        {
            var product = CreateProduct(
                productId: productId,
                productName: "Bebra",
                productPrice: 15.6m,
                productWeight: 12,
                productCategory: ProductCategory.Chemicals,
                productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
                warehouseId: 1613);
            
            products.Add(product);
            _repository.Insert(product);
        }
        
        Assert.Equal(products, _repository.GetAll().Values);
    }

    [Fact]
    public void InsertTest()
    {
        var product = CreateProduct(
            productId: 1,
            productName: "Bebra",
            productPrice: 15.6m,
            productWeight: 12,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);
        
        _repository.Insert(product);

        Assert.Contains(product, _repository.GetAll().Values);
    }

    [Fact]
    public void InsertSameProduct_ThrowAlreadyExistsRepositoryException_Test()
    {
        var product = CreateProduct(
            productId: 1,
            productName: "Bebra",
            productPrice: 15.6m,
            productWeight: 12,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);
        
        _repository.Insert(product);

        Assert.Throws<RepositoryException>(() =>
        {
            _repository.Insert(product);
        });
    }

    [Fact]
    public void InsertProductWithSameId_ThrowAlreadyExistsRepositoryException_Test()
    {
        var product = CreateProduct(
            productId: 1,
            productName: "Bebra",
            productPrice: 15.6m,
            productWeight: 12,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);
        
        var otherProduct = CreateProduct(
            productId: 1,
            productName: "OtherBebra",
            productPrice: 1.0m,
            productWeight: 1,
            productCategory: ProductCategory.Food,
            productManufactureDate: DateOnly.MaxValue,
            warehouseId: 3);
        
        _repository.Insert(product);

        Assert.Throws<RepositoryException>(() =>
        {
            _repository.Insert(otherProduct);
        });
    }

    [Fact]
    public void DeleteByIdProductTest()
    {
        var product = CreateProduct(
            productId: 1,
            productName: "Bebra",
            productPrice: 15.6m,
            productWeight: 12,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);

        _repository.Insert(product);
        
        _repository.DeleteById(new ProductId(1));
        
        Assert.DoesNotContain(product, _repository.GetAll().Values);
    }

    [Fact]
    public void DeleteById_ThrowProductDoesNotExistRepositoryException_Test()
    {
        Assert.Throws<RepositoryException>(() =>
        {
            _repository.DeleteById(new ProductId(1));
        });
    }

    [Fact]
    public void UpdateTest()
    {
        var productId = 1;
        
        var product = CreateProduct(
            productId: productId,
            productName: "Bebra",
            productPrice: 15.6m,
            productWeight: 12,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);

        _repository.Insert(product);

        var newPrice = 16823.12m;
        double newWeight = 123;

        var updatedProduct = CreateProduct(
            productId: productId,
            productName: "Bebra",
            productPrice: newPrice,
            productWeight: newWeight,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);

        _repository.Update(updatedProduct);
        
        Assert.Equal(updatedProduct, _repository.GetById(new ProductId(productId)));
    }

    [Fact]
    public void Update_ThrowProductDoesNotExistRepositoryException_Test()
    {
        var productId = 1;

        var newPrice = 16823.12m;
        double newWeight = 123;

        var updatedProduct = CreateProduct(
            productId: productId,
            productName: "Bebra",
            productPrice: newPrice,
            productWeight: newWeight,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);

        Assert.Throws<RepositoryException>(() =>
        {
            _repository.Update(updatedProduct);
        });
    }

    private Product CreateProduct(
        long productId,
        string productName,
        decimal productPrice,
        double productWeight,
        ProductCategory productCategory,
        DateOnly productManufactureDate,
        long warehouseId)
    {
        var product = new Product(
            new ProductId(productId),
            new ProductName(productName),
            new ProductPrice(productPrice),
            new ProductWeight(productWeight),
            productCategory,
            productManufactureDate,
            new WarehouseId(warehouseId));

        return product;
    }
}