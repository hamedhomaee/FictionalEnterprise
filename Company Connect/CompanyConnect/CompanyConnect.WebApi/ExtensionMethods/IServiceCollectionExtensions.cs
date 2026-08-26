using CompanyConnect.Core.Domain.RepositoryContracts;
using CompanyConnect.Infrastructure;
using CompanyConnect.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CompanyConnect.WebApi.ExtensionMethods;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAllServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });

        services
            .AddScoped<IEmployeeRepository, EmployeeRepository>();

        return services;
    }
}