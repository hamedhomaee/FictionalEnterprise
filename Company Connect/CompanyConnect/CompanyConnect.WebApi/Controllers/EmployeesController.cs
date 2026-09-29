using CompanyConnect.Core.Domain.Entities;
using CompanyConnect.Core.ServiceContracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CompanyConnect.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(
    IEmployeeService employeeService,
    IValidator<Employee> employeeValidator,
    ILogger<EmployeesController> logger
) : ControllerBase
{
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly IValidator<Employee> _employeeValidator = employeeValidator;
    private readonly ILogger<EmployeesController> _logger = logger;

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Employee>> CreateEmployeeAsync(Employee employee)
    {
        // Validate
        await _employeeValidator.ValidateAndThrowAsync(employee);

        // Add to data store
        await _employeeService.AddEmployeeAsync(employee);

        // Send the response
        return Created();
    }
}