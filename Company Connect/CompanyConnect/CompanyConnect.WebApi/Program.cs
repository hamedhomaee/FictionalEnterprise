using CompanyConnect.WebApi.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

// Adding services via extension method
builder.Services.AddAllServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();