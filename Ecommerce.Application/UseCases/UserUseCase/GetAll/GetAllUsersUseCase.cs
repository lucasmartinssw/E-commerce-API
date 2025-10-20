using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Repositories;
using System.Linq; 
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.UserUseCase.GetAll;

public class GetAllUsersUseCase
{
    private readonly IUserRepository _repository;

    public GetAllUsersUseCase(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseAllUsersJson> Execute()
    {
        var users = await _repository.GetAll();

        return new ResponseAllUsersJson
        {
        Users = users.Select(user => new ResponseUserJson{
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            }).ToList()
        };
    }
}