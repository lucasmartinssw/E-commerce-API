using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Products.Delete;

public class DeleteProductUseCase
{
    private readonly IProductRepository _productRepository;

    public DeleteProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Execute(long productId)
    {
        
        var product = await _productRepository.GetByIdTracked(productId);
        if (product == null)
        {

            throw new ValidationErrorsException(new List<string> { "Product not found." });
        }
        await _productRepository.Delete(product);
    }
}