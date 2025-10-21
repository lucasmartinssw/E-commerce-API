using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.DataAccess;
public class UserRepository : IUserRepository
{
    private readonly EcommerceDbContext _context;

    public UserRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsUserWithEmail(string email)
    {
        return await _context.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task<List<User>> GetAll()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<List<Address>> GetAllByUserId(long userId)
    {
        return await _context.Addresses
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }
}