using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Categories.Create;

public class CreateCategoryUseCase
{
    private readonly ICategoryRepository _repository;

    public CreateCategoryUseCase(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseCategoryJson> Execute(RequestCreateCategoryJson request)
    {
        
        Validate(request);

     
        var slug = GenerateSlug(request.Name);

        
        if (await _repository.ExistsByName(request.Name))
        {
            throw new ValidationErrorsException(new List<string> { "Uma categoria com este nome já existe." });
        }
        if (await _repository.ExistsBySlug(slug))
        {
            throw new ValidationErrorsException(new List<string> { "O 'slug' gerado a partir deste nome já existe." });
        }

        
        var category = new Category
        {
            Name = request.Name,
            Slug = slug
        };

        
        await _repository.Add(category);

        
        return new ResponseCategoryJson
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug
        };
    }

    private void Validate(RequestCreateCategoryJson request)
    {
        var validator = new CreateCategoryValidator();
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