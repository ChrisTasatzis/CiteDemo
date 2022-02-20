using CiteDemoBL.Models;
using CiteDemoBL.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load configuration for the SQLServer context
builder.Services.AddDbContext<CiteDemoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CiteDemoDB")));

// Dependency Injection for the services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAttributeService, AttributeService>();

// This is needed in order to run accept API calls from the front end
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:7204",
                                "http://127.0.0.1:5204",
                                "https://localhost:7204")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
