using CompanyConnect.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyConnect.Infrastructure;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
    }
}