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
}