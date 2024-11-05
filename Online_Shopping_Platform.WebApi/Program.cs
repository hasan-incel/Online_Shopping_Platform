using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Online_Shopping_Platform.Business.DataProtection;
using Online_Shopping_Platform.Business.Operations.Order;
using Online_Shopping_Platform.Business.Operations.Product;
using Online_Shopping_Platform.Business.Operations.Setting;
using Online_Shopping_Platform.Business.Operations.User;
using Online_Shopping_Platform.Data.Context;
using Online_Shopping_Platform.Data.Repositories;
using Online_Shopping_Platform.Data.UnitOfWork;
using Online_Shopping_Platform.WebApi.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add controller services for API endpoints
builder.Services.AddControllers();
// Add support for API documentation using Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Configure JWT Bearer authentication for Swagger
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Jwt Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Please enter a valid token",

        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    // Add security definition to Swagger
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    // Enforce security requirement for JWT authentication
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// Add custom data protection service
builder.Services.AddScoped<IDataProtection, DataProtection>();

// Define the directory to store data protection keys
var keysDirectory = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys"));

// Configure data protection to persist keys to file system
builder.Services.AddDataProtection()
       .SetApplicationName("Online_Shopping_Platform")
       .PersistKeysToFileSystem(keysDirectory);

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,  // Validate token issuer
                        ValidateAudience = true,  // Validate token audience
                        ValidateLifetime = true,  // Validate token expiration
                        ValidateIssuerSigningKey = true,  // Validate the signing key
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Get from config
                        ValidAudience = builder.Configuration["Jwt:Audience"],  // Get from config

                        // Use a secret key for signing JWT tokens
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
                    };
                });

// Configure database context with connection string
var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<OnlineShoppingPlatformDbContext>(options => options.UseSqlServer(connectionString));

// Register repository and service layer dependencies
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));  // Generic repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();  // Unit of work for transactional consistency
builder.Services.AddScoped<IUserService, UserManager>();  // User-related business logic
builder.Services.AddScoped<IProductService, ProductManager>();  // Product-related business logic
builder.Services.AddScoped<IOrderService, OrderManager>();  // Order-related business logic
builder.Services.AddScoped<ISettingService, SettingManager>();  // Setting-related business logic

builder.Services.AddLogging();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Enable Swagger in development
    app.UseSwaggerUI();  // UI for interacting with API
}

app.UseMiddleware<LoggingMiddleware>();


app.UseMaintenanceMode();  // Custom middleware for maintenance mode
app.UseHttpsRedirection();  // Redirect HTTP to HTTPS

app.UseAuthentication();  // Enable authentication middleware
app.UseAuthorization();  // Enable authorization middleware

app.MapControllers();  // Map API controllers to routes

app.Run();  // Start the web application
