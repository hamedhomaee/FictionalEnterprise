using System.Diagnostics;
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
    IValidator<CreateEmployeeDto> createEmployeeValidator,
    IValidator<UpdateEmployeeDto> updateEmployeeValidator,
    IMapper mapper,
    ILogger<EmployeesController> logger
) : ControllerBase
{
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly IValidator<CreateEmployeeDto> _createEmployeeValidator = createEmployeeValidator;
    private readonly IValidator<UpdateEmployeeDto> _updateEmployeeValidator = updateEmployeeValidator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<EmployeesController> _logger = logger;

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Employee>> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
    {
        var activity = Activity.Current;

        _logger.LogInformation(
            "Trace ID: {TraceId} - Creating Employee with email: {Email}",
            activity?.TraceId,
            createEmployeeDto.Email);

        await _createEmployeeValidator.ValidateAndThrowAsync(createEmployeeDto);

        Employee employee = _mapper.Map<Employee>(createEmployeeDto);

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

    [HttpGet]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<Employee>>> GetEmployeesPagedAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest("Invalid pagination parameters.");

        var employees = await _employeeService.GetEmployeesPagedAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        return Ok(employees);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Employee>> UpdateEmployeeAsync(
        int id,
        UpdateEmployeeDto updateEmployeeDto)
    {
        var activity = Activity.Current;

        _logger.LogInformation(
            "Trace ID: {TraceId} - Updating Employee with email: {Email}",
            activity?.TraceId,
            updateEmployeeDto.Email);

        await _updateEmployeeValidator.ValidateAndThrowAsync(updateEmployeeDto);

        Employee? employee = await _employeeService.GetEmployeeByIdAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        _mapper.Map(updateEmployeeDto, employee);

        await _employeeService.UpdateEmployeeAsync(employee);

        return NoContent();
    }
}