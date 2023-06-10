using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
}