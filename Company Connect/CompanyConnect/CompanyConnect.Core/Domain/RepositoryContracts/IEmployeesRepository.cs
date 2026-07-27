using CompanyConnect.Core.Domain.Entities;

namespace CompanyConnect.Core.Domain.RepositoryContracts;

public interface IEmployeesRepository
{
    public Task<Employee?> GetEmployeeAsync(int employeeId); 
    public Task<IReadOnlyList<Employee>> GetEmployeesAsync(); 
    public Task AddEmployeeAsync(Employee employee); 
    public Task UpdateEmployeeAsync(Employee employee); 
    public Task DeleteEmployeeAsync(Employee employee);
}