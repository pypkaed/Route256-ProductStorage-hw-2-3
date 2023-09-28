using System.Text;
using FluentValidation;
using FluentValidation.Results;
using Products.Requests;

namespace Products.Validators;

public class RequestValidator
{
    private readonly IValidator<CreateProductRequest> _createProductValidator;
    private readonly IValidator<GetProductByIdRequest> _getProductByIdValidator;
    private readonly IValidator<DeleteProductByIdRequest> _deleteProductByIdValidator;
    private readonly IValidator<UpdateProductPriceRequest> _updateProductPriceValidator;

    public RequestValidator(
        CreateProductRequestValidator createProductValidator,
        GetProductByIdRequestValidator getProductByIdValidator, 
        DeleteProductByIdRequestValidator deleteProductByIdValidator, 
        UpdateProductPriceRequestValidator updateProductPriceValidator)
    {
        _createProductValidator = createProductValidator;
        _getProductByIdValidator = getProductByIdValidator;
        _deleteProductByIdValidator = deleteProductByIdValidator;
        _updateProductPriceValidator = updateProductPriceValidator;
    }

    public void Validate(CreateProductRequest request)
    {
        var result = _createProductValidator.Validate(request);
        if (result.IsValid) return;

        ThrowValidationException(result.Errors);

    }
    public void Validate(GetProductByIdRequest request)
    {
        var result = _getProductByIdValidator.Validate(request);
        if (result.IsValid) return;

        ThrowValidationException(result.Errors);
    }
    public void Validate(DeleteProductByIdRequest request)
    {
        var result = _deleteProductByIdValidator.Validate(request);
        if (result.IsValid) return;

        ThrowValidationException(result.Errors);
    }
    public void Validate(UpdateProductPriceRequest request)
    {
        var result = _updateProductPriceValidator.Validate(request);
        if (result.IsValid) return;

        ThrowValidationException(result.Errors);
    }

    private static void ThrowValidationException(List<ValidationFailure> errors)
    {
        var errorStack = new StringBuilder();
        foreach (var error in errors)
        {
            errorStack.Append(error.ErrorMessage + "\n");
        }
        
        // TODO: excepton
        throw new Exception(errorStack.ToString());
    }
}