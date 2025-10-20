using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Categories.GetAll;

public class GetAllCategoriesUseCase
{
    private readonly ICategoryRepository _repository;

    public GetAllCategoriesUseCase(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseAllCategoriesJson> Execute()
    {
        var categories = await _repository.GetAll();

        return new ResponseAllCategoriesJson
        {
            Categories = categories.Select(c => new ResponseCategoryJson
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug
            }).ToList()
        };
    }
}