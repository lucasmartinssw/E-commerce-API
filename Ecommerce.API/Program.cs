using Ecommerce.API.Middleware;
using Ecommerce.Application.UseCases.UserUseCase.Register;
using Ecommerce.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.DataAccess;
using Ecommerce.Application.UseCases.UserUseCase.Login;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Caso de Uso ao container de injeção de dependência
builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<LoginUseCase>();

var app = builder.Build();

// Add o nosso middleware para tratar exceções de forma global
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
