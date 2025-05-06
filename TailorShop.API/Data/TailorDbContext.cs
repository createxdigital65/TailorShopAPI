using Microsoft.EntityFrameworkCore;
using TailorShop.API.Models;

namespace TailorShop.API.Data;

public class TailorDbContext : DbContext
{
    public TailorDbContext(DbContextOptions<TailorDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Measurement> Measurements => Set<Measurement>();
}
