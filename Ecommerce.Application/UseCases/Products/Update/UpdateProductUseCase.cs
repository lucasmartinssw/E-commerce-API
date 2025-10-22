using Ecommerce.Application.UseCases.Products.Create; 
using Ecommerce.Communication.Requests;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Products.Update;

public class UpdateProductUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductUseCase(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task Execute(long productId, RequestCreateProductJson request)
    {
        
        Validate(request);

        var product = await _productRepository.GetByIdTracked(productId);
        if (product == null)
        {
            throw new ValidationErrorsException(new List<string> { "Produto não encontrado." });
        }

        if (product.CategoryId != request.CategoryId)
        {
            var categoryExists = await _categoryRepository.Exists(request.CategoryId);
            if (!categoryExists)
            {
                throw new ValidationErrorsException(new List<string> { "Nova categoria não encontrada." });
            }
        }

        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.CategoryId = request.CategoryId;
        product.UpdatedAt = DateTime.UtcNow; 
        await _productRepository.Update(product);
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