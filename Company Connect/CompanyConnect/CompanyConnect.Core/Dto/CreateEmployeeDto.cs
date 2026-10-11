namespace CompanyConnect.Core.Dto;

public sealed record CreateEmployeeDto(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    DateTime HireDate,
    string Department,
    string JobTitle,
    decimal Salary
);