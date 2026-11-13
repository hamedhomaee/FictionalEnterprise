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
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == employeeId);

        return employee;
    }

    public async Task<IReadOnlyList<Employee>> GetEmployeesPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var employees = await _applicationDbContext
            .Employees
                .AsNoTracking()
                .OrderBy(e => e.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

        return employees;
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        var employeeToUpdate = await _applicationDbContext
            .Employees
                .SingleOrDefaultAsync(e => e.Id == employee.Id)
                ?? throw new EntityNotFoundException();

        if (!string.IsNullOrWhiteSpace(employee.FirstName))
            employeeToUpdate.FirstName = employee.FirstName;

        if (!string.IsNullOrWhiteSpace(employee.LastName))
            employeeToUpdate.LastName = employee.LastName;

        if (!string.IsNullOrWhiteSpace(employee.Email))
            employeeToUpdate.Email = employee.Email;

        if (!string.IsNullOrWhiteSpace(employee.PhoneNumber))
            employeeToUpdate.PhoneNumber = employee.PhoneNumber;

        if (employee.HireDate != default)
            employeeToUpdate.HireDate = employee.HireDate;

        if (!string.IsNullOrWhiteSpace(employee.Department))
            employeeToUpdate.Department = employee.Department;

        if (!string.IsNullOrWhiteSpace(employee.JobTitle))
            employeeToUpdate.JobTitle = employee.JobTitle;

        if (employee.Salary != default)
            employeeToUpdate.Salary = employee.Salary;

        if (employee.IsEmploymentTerminated != default)
            employeeToUpdate.IsEmploymentTerminated = employee.IsEmploymentTerminated;

        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> IsEmployeeDuplicateAsync(string email)
    {
        return await _applicationDbContext
            .Employees
                .AsNoTracking()
                    .AnyAsync(e => e.Email == email);
    }
}