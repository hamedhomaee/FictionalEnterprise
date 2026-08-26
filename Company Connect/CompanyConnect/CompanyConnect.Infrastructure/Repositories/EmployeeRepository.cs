using CompanyConnect.Core.Domain.Entities;
using CompanyConnect.Core.Domain.RepositoryContracts;
using FictionalEnterprise.Shared.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompanyConnect.Infrastructure.Repositories;

public class EmployeeRepository(
    ApplicationDbContext applicationDbContext,
    ILogger<EmployeeRepository> logger
) : IEmployeeRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly ILogger<EmployeeRepository> _logger = logger;

    public async Task AddEmployeeAsync(Employee employee)
    {
        if (employee == null)
        {
            throw new Exception("Requested employee was null");
        }

        try
        {
            _applicationDbContext.Employees.Add(employee);

            var result = await _applicationDbContext.SaveChangesAsync();

            if (result < 1)
            {
                throw new Exception("Number of affected rows was less than one");
            }
        }
        catch (Exception ex)
        {
            _logger.LogErrorClassNamePrepended($"Error while adding the employee into the database - {ex}");

            throw;
        }
    }

    public async Task DeleteEmployeeAsync(int employeeId)
    {
        Employee employee;

        if (employeeId == 0)
        {
            throw new Exception("Employee ID was not specified");
        }

        try
        {
            employee = await _applicationDbContext.Employees.SingleAsync(e => e.Id == employeeId);
        }
        catch (Exception ex)
        {
            _logger.LogErrorClassNamePrepended($"Error while fetching the requested employee from the database - {ex}");

            throw;
        }

        try
        {
            _applicationDbContext.Employees.Remove(employee);

            var result = await _applicationDbContext.SaveChangesAsync();

            if (result < 1)
            {
                throw new Exception("Number of affected rows was less than one");
            }
        }
        catch (Exception ex)
        {
            _logger.LogErrorClassNamePrepended($"Error while adding the employee into the database - {ex}");

            throw;
        }
    }

    public async Task<Employee?> GetEmployeeAsync(int employeeId)
    {
        if (employeeId == 0)
        {
            throw new Exception("Employee ID was not specified");
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
            _logger.LogErrorClassNamePrepended($"Error while fetching the employee from the database - {ex}");

            throw;
        }
    }

    public async Task<IReadOnlyList<Employee>> GetEmployeesAsync()
    {
        try
        {
            var employees = await _applicationDbContext.Employees.AsNoTracking().ToListAsync();

            return employees;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while fetching employees from the database - {ex}");

            throw;
        }
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        try
        {
            var employeeToUpdate = await _applicationDbContext
                .Employees
                    .SingleOrDefaultAsync(e => e.Id == employee.Id);

            employeeToUpdate!.FirstName = employee.FirstName;
            employeeToUpdate!.LastName = employee.LastName;
            employeeToUpdate!.Email = employee.Email;
            employeeToUpdate!.PhoneNumber = employee.PhoneNumber;
            employeeToUpdate!.HireDate = employee.HireDate;
            employeeToUpdate!.Department = employee.Department;
            employeeToUpdate!.JobTitle = employee.JobTitle;
            employeeToUpdate!.Salary = employee.Salary;
            employeeToUpdate!.IsEmploymentTerminated = employee.IsEmploymentTerminated;
            employeeToUpdate!.CreatedAt = employee.CreatedAt;
            employeeToUpdate!.UpdatedAt = employee.UpdatedAt;

            var result = await _applicationDbContext.SaveChangesAsync();

            if (result < 1)
            {
                throw new Exception("Number of affected rows was less than one");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while updating the employee in the database - {ex}");

            throw;
        }
    }
}