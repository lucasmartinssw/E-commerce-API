using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums; 
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Ecommerce.Infrastructure.DataAccess;

public class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options) { }
    static EcommerceDbContext()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<UserRoleType>();
        NpgsqlConnection.GlobalTypeMapper.MapEnum<OrderStatusType>();
        NpgsqlConnection.GlobalTypeMapper.MapEnum<PaymentStatusType>();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

      
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Password).HasColumnName("\"password\"");
            entity.Property(e => e.Role)
                .HasConversion<string>()
                .HasDefaultValue(UserRoleType.customer);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.Slug).IsUnique();
        });

        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Status)
                .HasConversion<string>()
                .HasDefaultValue(OrderStatusType.pending);

            entity.Property(e => e.OrderDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });


        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasIndex(e => new { e.OrderId, e.ProductId }).IsUnique();
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.HasOne(p => p.Order)
                  .WithOne(o => o.Payment)
                  .HasForeignKey<Payment>(p => p.OrderId); 

            entity.Property(e => e.Status)
                .HasConversion<string>()
                .HasDefaultValue(PaymentStatusType.pending);
        });

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}