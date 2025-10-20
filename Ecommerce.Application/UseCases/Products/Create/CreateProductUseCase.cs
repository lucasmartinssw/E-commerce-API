using Ecommerce.Application.UseCases.Products.Create; 
using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Application.UseCases.Products.Create;

public class CreateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductUseCase(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ResponseCreatedProductJson> Execute(RequestCreateProductJson request)
    {
        Validate(request);

        var categoryExists = await _categoryRepository.Exists(request.CategoryId);
        if (!categoryExists)
        {
            throw new ValidationErrorsException(new List<string> { "Categoria não encontrada." });
        }

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CategoryId = request.CategoryId
        };

        await _productRepository.Add(product);

        return new ResponseCreatedProductJson
        {
            Id = product.Id,
            Name = product.Name
        };
    }

    private void Validate(RequestCreateProductJson request)
    {
        var validator = new CreateProductValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationErrorsException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }
}