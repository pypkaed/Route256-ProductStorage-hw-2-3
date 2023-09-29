using ProductsDao.Models;

namespace ProductsDao.Exceptions;

public class RepositoryException : Exception
{
    private RepositoryException(string message) : base(message) {}

    public static RepositoryException ProductAlreadyExists(ProductId id)
        => new RepositoryException($"Product with id {id} already exists.");
    public static RepositoryException ProductDoesNotExists(ProductId id)
        => new RepositoryException($"Product with id {id} doesn't exists.");
}