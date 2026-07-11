namespace CompanyConnect.Core.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public string Department { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public bool IsEmploymentTerminated { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}