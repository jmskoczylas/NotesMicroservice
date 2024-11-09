using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
if (string.IsNullOrEmpty(path))
{
    throw new InvalidOperationException("The assembly location could not be determined.");
}

builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile(Path.Combine(path, "appsettings.json"), optional: true, reloadOnChange: true);

// Add services to the container
builder.Services.AddInfrastructureServices();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1", Version = "v1" });
});




var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers(); 

app.Run();
