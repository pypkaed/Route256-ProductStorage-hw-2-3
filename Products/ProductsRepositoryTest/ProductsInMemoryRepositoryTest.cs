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
    public void GetById_Success()
    {
        // Arrange
        var productId = 1;
        
        // Act
        var expected = CreateProduct(
            productId: productId,
            productName: "Bebra",
            productPrice: 15.6m,
            productWeight: 12,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);
        _repository.Insert(expected);
        
        // Assert
        var actual = _repository.GetById(new ProductId(productId));
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void GetById_ThrowProductDoesNotExistRepositoryException()
    {
        Assert.Throws<RepositoryException>(() =>
        {
            _repository.GetById(new ProductId(1));
        });
    }
    
    [Fact]
    public void GetAllTest_Success()
    {
        // Arrange
        var expected = new Dictionary<ProductId, Product>();
        
        // Act
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
            
            expected.Add(new ProductId(productId), product);
            _repository.Insert(product);
        }

        // Assert
        var actual = _repository.GetAll();
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void InsertTest_Success()
    {
        // Arrange
        var expected = CreateProduct(
            productId: 1,
            productName: "Bebra",
            productPrice: 15.6m,
            productWeight: 12,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);
        
        // Act
        _repository.Insert(expected);
    
        // Assert
        var actual = _repository.GetAll().Values;
        Assert.Contains(expected, actual);
    }
    
    [Fact]
    public void InsertProduct_SameProduct_ThrowAlreadyExistsRepositoryException()
    {
        // Arrange
        var product = CreateProduct(
            productId: 1,
            productName: "Bebra",
            productPrice: 15.6m,
            productWeight: 12,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);
        
        // Act
        _repository.Insert(product);
    
        // Assert
        Assert.Throws<RepositoryException>(() =>
        {
            _repository.Insert(product);
        });
    }
    
    [Fact]
    public void InsertProduct_WithSameId_ThrowAlreadyExistsRepositoryException()
    {
        // Arrange
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
        
        // Act
        _repository.Insert(product);
        
        // Assert
        Assert.Throws<RepositoryException>(() =>
        {
            _repository.Insert(otherProduct);
        });
    }
    
    [Fact]
    public void DeleteById_Success()
    {
        // Arrange
        var expected = CreateProduct(
            productId: 1,
            productName: "Bebra",
            productPrice: 15.6m,
            productWeight: 12,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);
        
        _repository.Insert(expected);
        
        // Act
        _repository.DeleteById(new ProductId(1));
        
        // Assert
        var actual = _repository.GetAll().Values;
        Assert.DoesNotContain(expected, actual);
    }
    
    [Fact]
    public void DeleteById_ThrowProductDoesNotExistRepositoryException()
    {
        Assert.Throws<RepositoryException>(() =>
        {
            _repository.DeleteById(new ProductId(1));
        });
    }
    
    [Fact]
    public void Update_Success()
    {
        // Arrange
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
    
        var expected = CreateProduct(
            productId: productId,
            productName: "Bebra",
            productPrice: newPrice,
            productWeight: newWeight,
            productCategory: ProductCategory.Chemicals,
            productManufactureDate: DateOnly.FromDateTime(DateTime.Now),
            warehouseId: 1613);
    
        // Act
        _repository.Update(expected);
        
        // Assert
        var actual = _repository.GetById(new ProductId(productId));
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void Update_ThrowProductDoesNotExistRepositoryException()
    {
        // Arrange
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
    
        // Assert
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