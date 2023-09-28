namespace Products.Requests;

public record UpdateProductPriceRequest(long ProductId, decimal NewPrice);