using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Products.GrpcServices;
using Products.Requests;
using Products.Responses;
using Products.Validators;
using ProductsBusiness.Dto;
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
    public ProductResponse CreateProduct([FromBody] CreateProductRequest request)
    {
        _validator.Validate(request);

        var productDto = _mapper.Map<ProductDto>(request);
        
        var result = _service.CreateProduct(productDto);
        
        var response = _mapper.Map<ProductResponse>(result);
        return response;
    }
    
    [HttpPost]
    [Route("[action]")]
    public ProductResponse GetProductById([FromBody] GetProductByIdRequest request)
    {
        _validator.Validate(request);
        
        var productDto = _service.GetProductById(request.ProductId);

        var response = _mapper.Map<ProductResponse>(productDto);
        return response;
    }  
    
    [HttpDelete]
    [Route("[action]")]
    public DeleteProductByIdResponse DeleteProductById([FromBody] DeleteProductByIdRequest request)
    {
        _validator.Validate(request);
        
        _service.DeleteProductById(request.ProductId);

        return new DeleteProductByIdResponse();
    }
    
    [HttpPut]
    [Route("[action]")]
    public ProductResponse UpdateProductPrice([FromBody] UpdateProductPriceRequest request)
    {
        _validator.Validate(request);
        
        var productDto = _service.UpdateProductPrice(request.ProductId, request.NewPrice);

        var response = _mapper.Map<ProductResponse>(productDto);
        return response;
    }
    
    [HttpPost]
    [Route("[action]")]
    public GetProductsFilteredResponse GetProductsFiltered(
        [FromBody] GetProductsFilteredRequest filters,
        [FromQuery] PaginationRequest pagination)
    {
        var filtersDto = _mapper.Map<FiltersDto>(filters);
        
        var filteredResult = _service.GetProductsFiltered(filtersDto);

        var page = _service.GetPage(pagination.PageNumber, pagination.PageLength, filteredResult);
        
        var productResponses = _mapper.Map<List<ProductResponse>>(page);

        var response = _mapper.Map<GetProductsFilteredResponse>(productResponses);
        return response;
    }
}