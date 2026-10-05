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

    [Fact]
    public async Task CreateEmployeesAsync_WhenValidEmployeeObjectIsPassed_ShouldAddEmployeeToDatabaseAndReturnSuccessResult()
    {
        // Arrange
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

        // Assert
        Assert.Multiple(
            () => Assert.Equal(HttpStatusCode.Created, response.StatusCode),
            async () =>
            {
                bool employeeExists = await _employeesControllerFixture
                    .ApplicationDbContext.Employees
                        .AnyAsync(e => e.Email == employee.Email);

                Assert.True(employeeExists);
            }
        );
    }
}