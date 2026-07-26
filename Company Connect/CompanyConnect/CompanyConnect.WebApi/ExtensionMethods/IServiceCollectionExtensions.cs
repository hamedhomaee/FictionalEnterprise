using CompanyConnect.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CompanyConnect.WebApi.ExtensionMethods;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });

        return services;
    }
}