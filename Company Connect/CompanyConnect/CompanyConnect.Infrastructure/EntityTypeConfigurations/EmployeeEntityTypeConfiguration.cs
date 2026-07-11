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
            .ValueGeneratedOnAdd()
            .HasColumnName("Id");
        
        builder
            .Property(e => e.FirstName)
            .HasMaxLength(50)
            .HasColumnName("FirstName")
            .IsRequired();

        builder
            .Property(e => e.LastName)
            .HasMaxLength(50)
            .HasColumnName("LastName")
            .IsRequired();

        builder
            .Property(e => e.Email)
            .HasMaxLength(50)
            .HasColumnName("Email")
            .IsRequired();
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
            .HasColumnName("HireDate")
            .IsRequired();

        builder
            .Property(e => e.JobTitle)
            .HasMaxLength(100)
            .HasColumnName("JobTitle")
            .IsRequired();
        
        builder
            .Property(e => e.Salary)
            .HasColumnType("decimal")
            .HasColumnName("Salary")
            .IsRequired();

        builder
            .Property(e => e.IsEmploymentTerminated)
            .HasColumnType("bit")
            .HasColumnName("IsEmploymentTerminated")
            .IsRequired();

        builder
            .Property(e => e.CreatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnType("datetime2")
            .HasColumnName("UpdatedAt")
            .IsRequired();
    }
}