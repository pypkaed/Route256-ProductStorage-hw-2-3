using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Products.Requests;
using Products.Responses;
using Products.Validators;
using ProductsBusiness.Services;

namespace Products.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController
{
    private readonly IMapper _mapper;
    private readonly RequestValidator _validator;
    private readonly IProductService _service;

    public ProductController(
        IProductService service,
        IMapper mapper,
        RequestValidator validator)
    {
        _service = service;
        _validator = validator;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("[action]")]
    public CreateProductResponse CreateProduct([FromBody] CreateProductRequest request)
    {
        _validator.Validate(request);
        var productDto = _service.CreateProduct(
            request.Id,
            request.Name,
            request.Price,
            request.Weight,
            request.Category,
            request.ManufactureDate,
            request.WarehouseId);
        var response = _mapper.Map<CreateProductResponse>(productDto);
        
        return response;
    }
    
    // [HttpPost]
    // [Route("[action]")]
    // public async Task<string> Test([FromBody] FilteredRequest filters)
    // {
    //     return "Bebra!";
    // }
}