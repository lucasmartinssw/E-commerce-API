using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Categories.Delete;

public class DeleteCategoryUseCase
{
    private readonly ICategoryRepository _repository;

    public DeleteCategoryUseCase(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task Execute(long categoryId)
    {
       
        if (categoryId <= 0)
        {
            throw new ValidationErrorsException(new List<string> { "ID da categoria é inválido." });
        }

        
        var category = await _repository.GetById(categoryId);
        if (category == null)
        {
            throw new ValidationErrorsException(new List<string> { "Categoria não encontrada." });
        }

       
        var isInUse = await _repository.IsInUse(categoryId);
        if (isInUse)
        {
            throw new ValidationErrorsException(new List<string> {
                "Não é possível excluir esta categoria, pois ela já está associada a produtos."
            });
        }

       
        await _repository.Delete(category);
    }
}