using CompanyConnect.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyConnect.EntityTypeConfigurations;

public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder
            .ToTable("Employees");

        builder
            .HasKey(e => e.Id);
        builder
            .Property(e =>e.Id)
            .HasColumnName("Id");
        
        builder
            .Property(e => e.FirstName)
            .HasMaxLength(50)
            .HasColumnName("FirstName");

        builder
            .Property(e => e.LastName)
            .HasMaxLength(50)
            .HasColumnName("LastName");

        builder
            .Property(e => e.Email)
            .HasMaxLength(50)
            .HasColumnName("Email");
        builder
            .HasIndex(e => e.Email)
            .IsUnique();

        builder
            .Property(e => e.PhoneNumber)
            .HasMaxLength(20)
            .HasColumnName("PhoneNumber");

        builder
            .Property(e => e.HireDate)
            .HasColumnType("date")
            .HasColumnName("HireDate");

        builder
            .Property(e => e.JobTitle)
            .HasMaxLength(100)
            .HasColumnName("JobTitle");
        
        builder
            .Property(e => e.Salary)
            .HasColumnType("decimal")
            .HasColumnName("Salary");

        builder
            .Property(e => e.IsEmploymentTerminated)
            .HasColumnType("bit")
            .HasColumnName("IsEmploymentTerminated");

        builder
            .Property(e => e.CreatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("CreatedAt");

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt");
    }
}