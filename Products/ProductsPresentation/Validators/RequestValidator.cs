using FluentValidation;
using Products.Requests;

namespace Products.Validators;

public class RequestValidator
{
    private readonly IValidator<CreateProductRequest> _createProductValidator;

    public RequestValidator(IValidator<CreateProductRequest> createProductValidator)
    {
        _createProductValidator = createProductValidator;
    }

    public void Validate(CreateProductRequest request)
    {
        _createProductValidator.Validate(request);
    }
}