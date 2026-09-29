using CompanyConnect.Core.Domain.Entities;
using CompanyConnect.Core.Domain.RepositoryContracts;
using CompanyConnect.Core.ServiceContracts;

namespace CompanyConnect.Core.Services;

public class EmployeeService(
    IEmployeeRepository employeeRepository
) : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

    public async Task AddEmployeeAsync(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        await _employeeRepository.AddEmployeeAsync(employee);
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        if (id == 0)
        {
            throw new ArgumentException("Employee ID cannot be zero");
        }

        Employee? result = await _employeeRepository.GetEmployeeAsync(id);

        if (result is null)
        {
            throw new EntryPointNotFoundException();
        }

        return result;
    }
}