using Application;
using Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
if (string.IsNullOrEmpty(path))
{
    throw new InvalidOperationException("The assembly location could not be determined.");
}

builder.Configuration
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();

// Add services to the container
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "NotesMicroservice.WebApi", Version = "v1" });
});

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NotesMicroservice.WebApi v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers(); 

app.Run();
