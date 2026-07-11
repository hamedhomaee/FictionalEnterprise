using CompanyConnect.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyConnect.EntityTypeConfigurations;

public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder
            .HasKey(e => e.Id);
        
        builder
            .Property(e => e.FirstName)
            .HasMaxLength(50);

        builder
            .Property(e => e.LastName)
            .HasMaxLength(50);

        builder
            .Property(e => e.Email)
            .HasMaxLength(50);
        builder
            .HasIndex(e => e.Email)
            .IsUnique();

        builder
            .Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder
            .Property(e => e.HireDate)
            .HasColumnType("date");

        builder
            .Property(e => e.JobTitle)
            .HasMaxLength(100);
        
        builder
            .Property(e => e.Salary)
            .HasColumnType("decimal");

        builder
            .Property(e => e.IsEmploymentTerminated)
            .HasColumnType("bit");

        builder
            .Property(e => e.CreatedAt)
            .HasColumnType("datetime2");

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnType("datetime2");
    }
}