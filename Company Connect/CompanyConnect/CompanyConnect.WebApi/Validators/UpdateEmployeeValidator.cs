using CompanyConnect.Core.Domain.Entities;
using CompanyConnect.Core.Domain.RepositoryContracts;
using CompanyConnect.Core.Dto;
using FluentValidation;

namespace CompanyConnect.WebApi.Validators;

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateEmployeeValidator(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;

        RuleFor(e => e)
            .Must(e =>
                !string.IsNullOrWhiteSpace(e.FirstName) ||
                !string.IsNullOrWhiteSpace(e.LastName) ||
                !string.IsNullOrWhiteSpace(e.Email) ||
                !string.IsNullOrWhiteSpace(e.PhoneNumber) ||
                !string.IsNullOrWhiteSpace(e.Department) ||
                !string.IsNullOrWhiteSpace(e.JobTitle) ||
                e.HireDate.HasValue ||
                e.Salary.HasValue)
            .WithMessage("At least one field must be provided for updating.");

        RuleFor(e => e.FirstName)
            .MaximumLength(50);

        RuleFor(e => e.LastName)
            .MaximumLength(50);

        RuleFor(e => e.Email)
            .EmailAddress()
            .CustomAsync(async (email, context, cancellationToken) =>
            {
                bool result = await _employeeRepository.IsEmployeeDuplicateAsync(email);

                if (result)
                {
                    context.AddFailure(
                        nameof(Employee.Email),
                        "An employee with this email already exists.");
                }
            })
            .When(e => !string.IsNullOrWhiteSpace(e.Email));

        RuleFor(e => e.PhoneNumber)
            .MaximumLength(20);

        RuleFor(e => e.Salary)
            .GreaterThanOrEqualTo(0);
    }
}