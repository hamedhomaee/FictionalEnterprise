using CompanyConnect.Core.Domain.Entities;
using CompanyConnect.Core.Domain.RepositoryContracts;
using FictionalEnterprise.Shared.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompanyConnect.Infrastructure.Repositories;

public class EmployeeRepository(
    ApplicationDbContext applicationDbContext,
    ILogger<EmployeeRepository> logger
) : IEmployeesRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly ILogger<EmployeeRepository> _logger = logger;

    public Task AddEmployeeAsync(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEmployeeAsync(int employeeId)
    {
        throw new NotImplementedException();
    }

    public async Task<Employee?> GetEmployeeAsync(int employeeId)
    {
        if (employeeId == 0)
        {
            throw new Exception("Employee ID was not specified.");
        }

        try
        {
            var employee = await _applicationDbContext
                .Employees
                    .SingleOrDefaultAsync(e => e.Id == employeeId);

            return employee;
        }
        catch (Exception ex)
        {
            _logger.LogErrorClassNamePrepended($"Error while fetching employee from database - {ex}");

            throw;
        }
    }

    public Task<IReadOnlyList<Employee>> GetEmployeesAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateEmployeeAsync(Employee employee)
    {
        throw new NotImplementedException();
    }
}