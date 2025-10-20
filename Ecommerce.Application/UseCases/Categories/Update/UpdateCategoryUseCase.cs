// Ecommerce.Application/UseCases/Categories/Update/UpdateCategoryUseCase.cs
using Ecommerce.Communication.Requests;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Categories.Update;

public class UpdateCategoryUseCase
{
    private readonly ICategoryRepository _repository;

    public UpdateCategoryUseCase(ICategoryRepository repository)
    {
        _repository = repository;
    }

  
    public async Task Execute(long categoryId, RequestUpdateCategoryJson request)
    {
       
        Validate(request);

    
        var category = await _repository.GetById(categoryId);
        if (category == null)
        {
            throw new ValidationErrorsException(new List<string> { "Categoria não encontrada." });
        }

        
        var newSlug = GenerateSlug(request.Name);

        if (await _repository.ExistsByNameExcludingId(request.Name, categoryId))
        {
            throw new ValidationErrorsException(new List<string> { "Uma categoria com este nome já existe." });
        }
        if (await _repository.ExistsBySlugExcludingId(newSlug, categoryId))
        {
            throw new ValidationErrorsException(new List<string> { "O 'slug' gerado a partir deste nome já existe." });
        }

        category.Name = request.Name;
        category.Slug = newSlug;

        await _repository.Update(category);
    }

    private void Validate(RequestUpdateCategoryJson request)
    {
        var validator = new UpdateCategoryValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationErrorsException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
    }

    private string GenerateSlug(string text)
    {
        var normalized = text.ToLowerInvariant().Trim();
        normalized = Regex.Replace(normalized, @"\s+", "-");
        normalized = Regex.Replace(normalized, @"[^a-z0-9-]", "");
        return normalized;
    }
}