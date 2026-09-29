using CompanyConnect.Core.Domain.Entities;

namespace CompanyConnect.Core.ServiceContracts;

public interface IEmployeeService
{
    public Task AddEmployeeAsync(Employee employee);
    public Task<Employee> GetEmployeeByIdAsync(int id);
}