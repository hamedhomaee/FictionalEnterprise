using CompanyConnect.WebApi.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

// Adding services
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();