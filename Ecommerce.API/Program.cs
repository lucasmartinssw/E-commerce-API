using Ecommerce.API.Middleware;
using Ecommerce.Application.Security.Token;
using Ecommerce.Application.Services.ViaCep;
using Ecommerce.Application.UseCases.Addresses.AddByCep;
using Ecommerce.Application.UseCases.Addresses.Delete;
using Ecommerce.Application.UseCases.Addresses.GetAll;
using Ecommerce.Application.UseCases.Addresses.Update;
using Ecommerce.Application.UseCases.Carts;
using Ecommerce.Application.UseCases.Carts.Clear;
using Ecommerce.Application.UseCases.Carts.DeleteItem;
using Ecommerce.Application.UseCases.Carts.UpdateItem;
using Ecommerce.Application.UseCases.Categories.Create;
using Ecommerce.Application.UseCases.Categories.Delete;
using Ecommerce.Application.UseCases.Categories.GetAll;
using Ecommerce.Application.UseCases.Categories.Update;
using Ecommerce.Application.UseCases.Orders.Checkout;
using Ecommerce.Application.UseCases.Products.Create;
using Ecommerce.Application.UseCases.Products.GetAllPaged;
using Ecommerce.Application.UseCases.UserUseCase.ChangePassword;
using Ecommerce.Application.UseCases.UserUseCase.GetAll;
using Ecommerce.Application.UseCases.UserUseCase.GetProfile;
using Ecommerce.Application.UseCases.UserUseCase.Login;
using Ecommerce.Application.UseCases.UserUseCase.Register;
using Ecommerce.Application.UseCases.UserUseCase.UpdateProfile;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Adiciona serviços ao container ---

// Configurações de Infraestrutura (Banco de Dados e Repositórios)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>(); 
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Configurações da Aplicação (Casos de Uso, Segurança e Services)
builder.Services.AddScoped<ViaCepService>();
builder.Services.AddSingleton<JwtTokenGenerator>();
builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<CreateProductUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<CreateCategoryUseCase>();
builder.Services.AddScoped<GetAllCategoriesUseCase>();
builder.Services.AddScoped<UpdateUserProfileUseCase>();
builder.Services.AddScoped<DeleteCategoryUseCase>();
builder.Services.AddScoped<UpdateCategoryUseCase>();
builder.Services.AddScoped<GetAllPagedProductsUseCase>();
builder.Services.AddScoped<GetUserProfileUseCase>();
builder.Services.AddScoped<ChangePasswordUseCase>();
builder.Services.AddScoped<AddAddressByCepUseCase>();
builder.Services.AddScoped<DeleteAddressUseCase>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ViaCepService>();
builder.Services.AddScoped<GetAllAddressesUseCase>();
builder.Services.AddScoped<UpdateAddressUseCase>();
builder.Services.AddScoped<AddCartItemUseCase>();
builder.Services.AddScoped<GetCartUseCase>();
builder.Services.AddScoped<UpdateCartItemUseCase>();
builder.Services.AddScoped<DeleteCartItemUseCase>();
builder.Services.AddScoped<ClearCartUseCase>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<CheckoutUseCase>();


// Configurações da API (Controllers e Swagger)
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta maneira: Bearer SEU_TOKEN"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configuração de Autenticação (JWT)
var securityKey = builder.Configuration.GetSection("Jwt:SecurityKey").Value;
var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey!));

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearer =>
{
    bearer.RequireHttpsMetadata = false;
    bearer.SaveToken = true;
    bearer.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = symmetricKey,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("admin"));
});

// --- Fim da adição de serviços ---

var app = builder.Build();

// --- Configura o pipeline de requisições HTTP ---

// O middleware de exceção deve vir primeiro
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();