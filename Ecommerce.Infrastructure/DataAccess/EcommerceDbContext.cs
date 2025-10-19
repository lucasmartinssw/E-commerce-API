// Ecommerce.Infrastructure/DataAccess/EcommerceDbContext.cs
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.DataAccess;

public class EcommerceDbContext : DbContext
{
    // Este construtor permite que a Injeção de Dependência
    // configure a conexão com o banco de dados (que está no Program.cs)
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options) { }

    // Esta linha mapeia a sua classe "User" do Domain
    // para uma tabela chamada "Users" no banco de dados.
    public DbSet<User> Users { get; set; }
}