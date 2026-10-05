using CompanyConnect.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CompanyConnect.IntegrationTests.TestFixtures;

public sealed class EmployeesControllerFixture : IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = null!;
    public ApplicationDbContext ApplicationDbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        HttpClient = new HttpClient
        {
            BaseAddress = new Uri(configuration["EmployeesControllerConfiguration:BaseUrl"]!)
        };

        ApplicationDbContext = new(new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(configuration.GetConnectionString("SqlServer"))
            .Options);
    }

    public async Task DisposeAsync()
    {
        // Removing the fake employee from database
        var fakeEmployee = await ApplicationDbContext.Employees
            .FirstOrDefaultAsync(e => e.Email == Constants.FakeEmailAddress);

        if (fakeEmployee is not null)
        {
            ApplicationDbContext.Employees.Remove(fakeEmployee);
            
            await ApplicationDbContext.SaveChangesAsync();
        }

        // Disposing unnecessary instances
        HttpClient?.Dispose();

        await ApplicationDbContext.DisposeAsync();
    }
}