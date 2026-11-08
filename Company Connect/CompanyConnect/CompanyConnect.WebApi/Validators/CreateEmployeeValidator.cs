using CompanyConnect.Core.Domain.Entities;
using CompanyConnect.Core.Domain.RepositoryContracts;
using CompanyConnect.Core.Dto;
using FluentValidation;

namespace CompanyConnect.WebApi.Validators;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;

    public CreateEmployeeValidator(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;

        RuleFor(e => e.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(e => e.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(e => e.Email)
            .NotEmpty()
            .MaximumLength(50)
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
            });

        RuleFor(e => e.PhoneNumber)
            .MaximumLength(20);

        RuleFor(e => e.HireDate)
            .NotEmpty();

        RuleFor(e => e.Salary)
            .GreaterThanOrEqualTo(0);
    }
}