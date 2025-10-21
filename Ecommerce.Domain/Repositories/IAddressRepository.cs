using Ecommerce.Domain.Entities;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Repositories;

public interface IAddressRepository
{
    Task Add(Address address);
    Task<List<Address>> GetAllByUserId(long userId);
    Task<Address?> GetById(long id);
    Task Update(Address address);
    Task Delete(Address address);
}