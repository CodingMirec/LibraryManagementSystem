using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Core.Interfaces.Repositories;
using LibraryManagementSystem.Core.Interfaces.Services;
using LibraryManagementSystem.Infrastructure.Repositories;
using LibraryManagementSystem.Infrastructure.Services;
using LibraryManagementSystem.Infrastructure.Middlewares;
using Serilog;
using FluentValidation.AspNetCore;
using LibraryManagementSystem.Infrastructure.Mappings;
using LibraryManagementSystem.Core.DTOs.Validators;
using FluentValidation;
using Microsoft.OpenApi.Models;
using LibraryManagementSystem.Core.DTOs.RequestDtos;

var builder = WebApplication.CreateBuilder(args);

// Register repositories
builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ILoansRepository, LoansRepository>();

// Register services
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ILoansService, LoansService>();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IValidator<BookRequestDto>, BookDTOValidator>();
builder.Services.AddScoped<IValidator<UserRequestDto>, UserDTOValidator>();
builder.Services.AddScoped<IValidator<LoanRequestDto>, LoanDTOValidator>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Library Management System API",
        Version = "v1",
        Description = "An API for managing library books."
    });
});

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management System API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
