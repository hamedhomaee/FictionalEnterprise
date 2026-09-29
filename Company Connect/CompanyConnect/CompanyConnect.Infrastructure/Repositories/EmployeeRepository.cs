using CompanyConnect.Core.CustomExceptions;
using CompanyConnect.Core.Domain.Entities;
using CompanyConnect.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace CompanyConnect.Infrastructure.Repositories;

public class EmployeeRepository(
    ApplicationDbContext applicationDbContext
) : IEmployeeRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task AddEmployeeAsync(Employee employee)
    {
        _applicationDbContext.Employees.Add(employee);

        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int employeeId)
    {
        Employee employee;

        employee = await _applicationDbContext
            .Employees
                .SingleOrDefaultAsync(e => e.Id == employeeId) ?? throw new EntityNotFoundException();

        _applicationDbContext.Employees.Remove(employee);

        var result = await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<Employee?> GetEmployeeAsync(int employeeId)
    {
        var employee = await _applicationDbContext
            .Employees
                .SingleOrDefaultAsync(e => e.Id == employeeId);

        return employee;
    }

    public async Task<IReadOnlyList<Employee>> GetEmployeesAsync()
    {
        var employees = await _applicationDbContext
            .Employees
                .AsNoTracking()
                    .ToListAsync();

        return employees;
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        var employeeToUpdate = await _applicationDbContext
            .Employees
                .SingleOrDefaultAsync(e => e.Id == employee.Id) ?? throw new EntityNotFoundException();

        employeeToUpdate.FirstName = employee.FirstName;
        employeeToUpdate.LastName = employee.LastName;
        employeeToUpdate.Email = employee.Email;
        employeeToUpdate.PhoneNumber = employee.PhoneNumber;
        employeeToUpdate.HireDate = employee.HireDate;
        employeeToUpdate.Department = employee.Department;
        employeeToUpdate.JobTitle = employee.JobTitle;
        employeeToUpdate.Salary = employee.Salary;
        employeeToUpdate.IsEmploymentTerminated = employee.IsEmploymentTerminated;
        employeeToUpdate.CreatedAt = employee.CreatedAt;
        employeeToUpdate.UpdatedAt = employee.UpdatedAt;

        var result = await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> IsEmployeeDuplicateAsync(string email)
    {
        return await _applicationDbContext
            .Employees
                .AsNoTracking()
                    .AnyAsync(e => e.Email == email);
    }
}