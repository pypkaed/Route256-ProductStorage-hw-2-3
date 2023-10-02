using AutoMapper;
using FluentValidation;
using Grpc.Core;
using ProductGrpc;
using ProductsBusiness.Dto;
using ProductsBusiness.Services;

namespace Products.GrpcServices;

public class ProductsGrpcService : ProductGrpcService.ProductGrpcServiceBase
{
    private readonly IProductService _service;
    private readonly IMapper _mapper;
    
    public ProductsGrpcService(
        IProductService service, 
        IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    public override async Task<ProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        var productDto = _mapper.Map<ProductDto>(request);
        var result = _service.CreateProduct(productDto);
        var response = _mapper.Map<ProductResponse>(result);

        await Task.CompletedTask;
        return response;
    }

    public override async Task<ProductResponse> GetProductById(GetProductByIdRequest request, ServerCallContext context)
    {
        var result = _service.GetProductById(request.Id);
        var response = _mapper.Map<ProductResponse>(result);

        await Task.CompletedTask;
        return response;
    }

    public override async Task<DeleteProductByIdResponse> DeleteProductById(DeleteProductByIdRequest request, ServerCallContext context)
    {
        _service.DeleteProductById(request.Id);

        await Task.CompletedTask;
        return new DeleteProductByIdResponse();
    }

    public override async Task<ProductResponse> UpdateProductPrice(UpdateProductPriceRequest request, ServerCallContext context)
    {
        var result = _service.UpdateProductPrice(request.Id, (decimal)request.Price);
        var response = _mapper.Map<ProductResponse>(result);

        await Task.CompletedTask;
        return response;
    }

    public override async Task<GetProductsFilteredResponse> GetProductsFiltered(GetProductsFilteredRequest request, ServerCallContext context)
    {
        var filtersDto = _mapper.Map<FiltersDto>(request);
        
        var products = _service.GetProductsFiltered(filtersDto);
        if (request.Pagination is not null) {
            products = _service.GetPage(request.Pagination.PageNumber, request.Pagination.PageLength, products);
        }
        
        var pageResponse = _mapper.Map<List<ProductResponse>>(products);
        var response = new GetProductsFilteredResponse();
        response.Products.AddRange(pageResponse);

        await Task.CompletedTask;
        return response;
    }
}