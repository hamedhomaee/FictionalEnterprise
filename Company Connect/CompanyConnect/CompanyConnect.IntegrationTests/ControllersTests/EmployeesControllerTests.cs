using System.Net;
using System.Net.Http.Json;
using CompanyConnect.Core.Domain.Entities;
using CompanyConnect.IntegrationTests.TestFixtures;
using Microsoft.EntityFrameworkCore;

namespace CompanyConnect.IntegrationTests.ControllersTests;

public class EmployeesControllerTests(
    EmployeesControllerFixture employeesControllerFixture
) : IClassFixture<EmployeesControllerFixture>
{
    private readonly EmployeesControllerFixture _employeesControllerFixture = employeesControllerFixture;
    private const string EmployeesEndpoint = "api/employees";

    #region CreateEmployee
    [Fact]
    public async Task CreateEmployeeAsync_WhenValidEmployeeObjectIsPassed_ShouldAddEmployeeToDatabaseAndReturnSuccessResult()
    {
        try
        {
            // Arrange
            var expectedValue = HttpStatusCode.Created;

            Employee employee = new()
            {
                FirstName = "Fake First Name",
                LastName = "Fake Last Name",
                Email = Constants.FakeEmailAddress,
                PhoneNumber = "0123456",
                HireDate = DateTime.Now,
                Department = "Fake Department",
                JobTitle = "Fake Job Title",
                Salary = 4000.9M,
                IsEmploymentTerminated = false,
                CreatedAt = DateTime.Now
            };

            HttpRequestMessage createEmployeeHttpRequestMessage = new(
                HttpMethod.Post,
                EmployeesEndpoint)
            {
                Content = JsonContent.Create(employee)
            };

            // Act
            var response = await _employeesControllerFixture
                .HttpClient.SendAsync(createEmployeeHttpRequestMessage);

            var actualValue = response.StatusCode;

            // Assert
            Assert.Equal(expectedValue, actualValue);

            bool exists = await _employeesControllerFixture
                .ApplicationDbContext
                    .Employees
                        .AnyAsync(e => e.Email == employee.Email);

            Assert.True(exists);
        }
        finally
        {
            await _employeesControllerFixture
                .ApplicationDbContext
                    .Employees.Where(e => e.Email == Constants.FakeEmailAddress)
                        .ExecuteDeleteAsync();
        }
    }

    [Fact]
    public async Task CreateEmployeeAsync_WhenRequiredFieldAreMissing_ShouldReturnBadRequestResult()
    {
        // Arrange
        var expectedValue = HttpStatusCode.BadRequest;

        Employee employee = new()
        {
            FirstName = "Fake First Name",
            LastName = "Fake Last Name",
            Email = "",
            PhoneNumber = "0123456",
            HireDate = DateTime.Now,
            Department = "Fake Department",
            JobTitle = "Fake Job Title",
            Salary = 4000.9M,
            IsEmploymentTerminated = false,
            CreatedAt = DateTime.Now
        };

        HttpRequestMessage createEmployeeHttpRequestMessage = new(
            HttpMethod.Post,
            EmployeesEndpoint)
        {
            Content = JsonContent.Create(employee)
        };

        // Act
        var response = await _employeesControllerFixture
            .HttpClient.SendAsync(createEmployeeHttpRequestMessage);

        var actualValue = response.StatusCode;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task CreateEmployeeAsync_WhenEmailAddressIsInvalid_ShouldReturnBadRequestResult()
    {
        // Arrange
        var expectedValue = HttpStatusCode.BadRequest;

        Employee employee = new()
        {
            FirstName = "Fake First Name",
            LastName = "Fake Last Name",
            Email = "invalid email address",
            PhoneNumber = "0123456",
            HireDate = DateTime.Now,
            Department = "Fake Department",
            JobTitle = "Fake Job Title",
            Salary = 4000.9M,
            IsEmploymentTerminated = false,
            CreatedAt = DateTime.Now
        };

        HttpRequestMessage createEmployeeHttpRequestMessage = new(
            HttpMethod.Post,
            EmployeesEndpoint)
        {
            Content = JsonContent.Create(employee)
        };

        // Act
        var response = await _employeesControllerFixture
            .HttpClient.SendAsync(createEmployeeHttpRequestMessage);

        var actualValue = response.StatusCode;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task CreateEmployeeAsync_WhenEmailAddressAlreadyExists_ShouldReturnBadRequestResult()
    {
        try
        {
            // Arrange
            var expectedValue = HttpStatusCode.BadRequest;

            Employee employee = new()
            {
                FirstName = "Fake First Name",
                LastName = "Fake Last Name",
                Email = Constants.FakeEmailAddress,
                PhoneNumber = "0123456",
                HireDate = DateTime.Now,
                Department = "Fake Department",
                JobTitle = "Fake Job Title",
                Salary = 4000.9M,
                IsEmploymentTerminated = false,
                CreatedAt = DateTime.Now
            };

            // Create the fake employee for the first time
            {
                HttpRequestMessage createEmployeeHttpRequestMessage = new(
                    HttpMethod.Post,
                    EmployeesEndpoint)
                {
                    Content = JsonContent.Create(employee)
                };

                var response = await _employeesControllerFixture
                    .HttpClient
                        .SendAsync(createEmployeeHttpRequestMessage);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }

            // Act
            {
                // Create the employee with the same email address
                HttpRequestMessage createEmployeeHttpRequestMessage = new(
                                    HttpMethod.Post,
                                    EmployeesEndpoint)
                {
                    Content = JsonContent.Create(employee)
                };

                var response = await _employeesControllerFixture
                    .HttpClient
                        .SendAsync(createEmployeeHttpRequestMessage);

                var actualValue = response.StatusCode;

                Assert.Equal(expectedValue, actualValue);
            }
        }
        finally
        {
            await _employeesControllerFixture
                .ApplicationDbContext
                    .Employees.Where(e => e.Email == Constants.FakeEmailAddress)
                        .ExecuteDeleteAsync();
        }
    }
    #endregion
}