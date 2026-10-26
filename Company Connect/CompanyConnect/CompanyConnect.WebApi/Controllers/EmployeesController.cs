using System.Diagnostics;
using System.Net;
using AutoMapper;
using CompanyConnect.Core.Domain.Entities;
using CompanyConnect.Core.Dto;
using CompanyConnect.Core.ServiceContracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CompanyConnect.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(
    IEmployeeService employeeService,
    IValidator<CreateEmployeeDto> employeeValidator,
    IMapper mapper,
    ILogger<EmployeesController> logger
) : ControllerBase
{
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly IValidator<CreateEmployeeDto> _employeeValidator = employeeValidator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<EmployeesController> _logger = logger;

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1873: Avoid potentially expensive logging",
        Justification = "Logging arguments are simple property accesses and inexpensive to evaluate.")]
    public async Task<ActionResult<Employee>> CreateEmployeeAsync(CreateEmployeeDto employeeDto)
    {
        var activity = Activity.Current;

        _logger.LogInformation(
            "Trace ID: {TraceId} - Creating Employee with email: {Email}",
            activity?.TraceId,
            employeeDto.Email
        );

        await _employeeValidator.ValidateAndThrowAsync(employeeDto);

        Employee employee = _mapper.Map<Employee>(employeeDto);

        await _employeeService.AddEmployeeAsync(employee);

        return Created();
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Employee>> GetEmployeeAsync(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var result = await _employeeService
            .GetEmployeeByIdAsync(id);
        
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}