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
    private int fakeEmployeeId;

    #region CreateEmployeeAsync
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

            // Assert (Since DbContext is used here, multiple assert causes concurrency issues)
            {
                Assert.Equal(expectedValue, actualValue);

                bool exists = await _employeesControllerFixture
                    .ApplicationDbContext
                        .Employees
                            .AnyAsync(e => e.Email == employee.Email);

                Assert.True(exists);
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
            await AddFakeEmployeeIntoDatabase();

            // Create the employee with the same email address
            HttpRequestMessage createEmployeeHttpRequestMessage = new(
                HttpMethod.Post,
                EmployeesEndpoint)
            {
                Content = JsonContent.Create(employee)
            };

            // Act
            var response = await _employeesControllerFixture
                .HttpClient
                    .SendAsync(createEmployeeHttpRequestMessage);

            var actualValue = response.StatusCode;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
        finally
        {
            await RemoveFakeEmployeeFromDatabase();
        }
    }
    #endregion

    #region GetEmployeeAsync
    [Fact]
    public async Task GetEmployeeAsync_WhenEmployeeExists_ShouldReturnOkResult()
    {
        // Arrange
        var expectedValue = HttpStatusCode.OK;

        // We have this pre-assumption that employer with ID=1 exists
        HttpRequestMessage getEmployeeHttpRequestMessage = new(
            HttpMethod.Get,
            $"{EmployeesEndpoint}/1");

        // Act
        var response = await _employeesControllerFixture
            .HttpClient.SendAsync(getEmployeeHttpRequestMessage);

        var actualValue = response.StatusCode;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task GetEmployeeAsync_WhenEmployeeDoesNotExist_ShouldReturnNotFoundResult()
    {
        // Arrange
        var expectedValue = HttpStatusCode.NotFound;

        // We get the last ID and increase it by one to make sure the employee does not exist
        int maxId = await _employeesControllerFixture
            .ApplicationDbContext
                .Employees
                    .MaxAsync(e => (int?)e.Id) ?? 0;

        int nonExistingEmployeeId = maxId + 1;

        HttpRequestMessage getEmployeeHttpRequestMessage = new(
            HttpMethod.Get,
            $"{EmployeesEndpoint}/{nonExistingEmployeeId}");

        // Act
        var response = await _employeesControllerFixture
            .HttpClient.SendAsync(getEmployeeHttpRequestMessage);

        var actualValue = response.StatusCode;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task GetEmployeeAsync_WhenIdZeroIsPassed_ShouldReturnBadRequestResult()
    {
        // Arrange
        var expectedValue = HttpStatusCode.BadRequest;

        HttpRequestMessage getEmployeeHttpRequestMessage = new(
            HttpMethod.Get,
            $"{EmployeesEndpoint}/0");

        // Act
        var response = await _employeesControllerFixture
            .HttpClient.SendAsync(getEmployeeHttpRequestMessage);

        var actualValue = response.StatusCode;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task GetEmployeeAsync_WhenIdIsNegative_ShouldReturnBadRequestResult()
    {
        // Arrange
        var expectedValue = HttpStatusCode.BadRequest;

        HttpRequestMessage getEmployeeHttpRequestMessage = new(
            HttpMethod.Get,
            $"{EmployeesEndpoint}/-1");

        // Act
        var response = await _employeesControllerFixture
            .HttpClient.SendAsync(getEmployeeHttpRequestMessage);

        var actualValue = response.StatusCode;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }
    #endregion

    #region GetEmployeesAsync
    [Fact]
    public async Task GetEmployeesAsync_WhenAnyEmployeeExists_ShouldReturnOkResult()
    {
        // Arrange
        var expectedValue = HttpStatusCode.OK;

        HttpRequestMessage getEmployeesHttpRequestMessage = new(
            HttpMethod.Get,
            $"{EmployeesEndpoint}");

        // Act
        var response = await _employeesControllerFixture
            .HttpClient.SendAsync(getEmployeesHttpRequestMessage);

        var actualValue = response.StatusCode;

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }
    #endregion

    #region UpdateEmployeeAsync
    [Fact]
    public async Task UpdateEmployeeAsync_WhenPerformedOnExistingEmployeeWithValidData_ShouldReturnNoContentResultAndValuesArePersisted()
    {
        try
        {
            // Arrange
            var expectedValue = HttpStatusCode.NoContent;

            Employee actualEmployee = new()
            {
                FirstName = "Updated Fake First Name",
                LastName = "Updated Fake Last Name",
                Email = "updatedfakeemail@example.com",
                PhoneNumber = "0123456789",
                HireDate = DateTime.Now,
                Department = "Updated Fake Department",
                JobTitle = "Updated Fake Job Title",
                Salary = 5000.5M,
                IsEmploymentTerminated = true,
                CreatedAt = DateTime.Now
            };

            // Create the fake employee for the first time
            await AddFakeEmployeeIntoDatabase();

            HttpRequestMessage updateEmployeeHttpRequestMessage = new(
                HttpMethod.Put,
                $"{EmployeesEndpoint}/{fakeEmployeeId}")
            {
                Content = JsonContent.Create(actualEmployee)
            };

            // Act
            var response = await _employeesControllerFixture
                .HttpClient.SendAsync(updateEmployeeHttpRequestMessage);

            var actualValue = response.StatusCode;

            // Assert
            Assert.Equal(expectedValue, actualValue);

            // Get the updated employee from the database
            Employee? expectedEmployee = await _employeesControllerFixture
                .ApplicationDbContext
                    .Employees
                        .AsNoTracking()
                            .FirstOrDefaultAsync(e => e.Id == fakeEmployeeId);

            Assert.True(AreUpdatedValuesPersisted(expectedEmployee!, actualEmployee));
        }
        finally
        {
            await RemoveFakeEmployeeFromDatabase();
        }
    }
    #endregion

    #region PrivateMethods
    private async Task AddFakeEmployeeIntoDatabase()
    {
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

        await _employeesControllerFixture
            .ApplicationDbContext
                .Employees
                    .AddAsync(employee);

        await _employeesControllerFixture
            .ApplicationDbContext
                .SaveChangesAsync();

        fakeEmployeeId = employee.Id;
    }

    private async Task RemoveFakeEmployeeFromDatabase()
    {
        await _employeesControllerFixture
                .ApplicationDbContext
                    .Employees.Where(e => e.Id == fakeEmployeeId)
                        .ExecuteDeleteAsync();
    }

    private bool AreUpdatedValuesPersisted(Employee expected, Employee actual)
    {
        return expected.FirstName == actual.FirstName
            && expected.LastName == actual.LastName
            && expected.Email == actual.Email
            && expected.PhoneNumber == actual.PhoneNumber
            && expected.HireDate == actual.HireDate
            && expected.Department == actual.Department
            && expected.Salary == actual.Salary
            && expected.JobTitle == actual.JobTitle
            && expected.CreatedAt == actual.CreatedAt
            && expected.IsEmploymentTerminated == actual.IsEmploymentTerminated;
    }
    #endregion
}