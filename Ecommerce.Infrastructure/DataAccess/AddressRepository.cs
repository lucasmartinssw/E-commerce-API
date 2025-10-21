
using Ecommerce.Domain.Entities; 
using Ecommerce.Domain.Repositories; 
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.DataAccess;

public class AddressRepository : IAddressRepository
{
    private readonly EcommerceDbContext _context;

    public AddressRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public async Task Add(Address address)
    {
        await _context.Addresses.AddAsync(address);

        
        await _context.SaveChangesAsync();
    }
    public async Task<List<Address>> GetAllByUserId(long userId)
    {
        return await _context.Addresses
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }
    public async Task<Address?> GetById(long id)
    {
        return await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task Update(Address address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Address address)
    {
        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
    }
}