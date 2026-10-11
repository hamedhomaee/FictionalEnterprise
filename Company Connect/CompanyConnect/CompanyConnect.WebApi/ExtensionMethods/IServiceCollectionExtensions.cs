using CompanyConnect.Core.Domain.RepositoryContracts;
using CompanyConnect.Core.ServiceContracts;
using CompanyConnect.Core.Services;
using CompanyConnect.Infrastructure;
using CompanyConnect.Infrastructure.Repositories;
using CompanyConnect.WebApi.ExceptionHandlers;
using CompanyConnect.WebApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CompanyConnect.WebApi.ExtensionMethods;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAllServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Controllers
        services.AddControllers();

        // DbContext
        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });

        // Validators
        services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();

        // Custom and global exception handlers
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Repositories
        services
            .AddScoped<IEmployeeRepository, EmployeeRepository>();

        // Services
        services
            .AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }
}