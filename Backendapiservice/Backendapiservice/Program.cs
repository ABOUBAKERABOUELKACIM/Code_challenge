using Microsoft.EntityFrameworkCore;
using Backendapiservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Backendapiservice.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backendapiservice.Infrastructure.Services;
using Microsoft.OpenApi.Models;
using Backendapiservice.Domain.Interfaces;
using Backendapiservice.Infrastructure.Events;
using FluentValidation.AspNetCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation;
using Backendapiservice.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Use In-Memory Database for development
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("DoctorOfficeManagement"));
// Add MediatR 
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "DoctorOfficeAPI",
            ValidAudience = "DoctorOfficeClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsAVeryLongSecretKeyForJWTTokenGeneration123456789012345678901234567890"))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:4173")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Doctor Office API", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
// Add FluentValidation
// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();
// Seed database with test data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Seed admin user

    if (!context.Admins.Any())
    {
        var admin = new Admin
        {
            FirstName = "System",
            LastName = "Administrator",
            Email = "admin@doctoroffice.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
            // Role will be set to "Admin" automatically in constructor
        };

        context.Admins.Add(admin);
        context.SaveChanges();
    }

    // Seed a test office
    if (!context.Offices.Any())
    {
        var office = new Office
        {
            Name = "Main Medical Center",
            Address = "123 Healthcare Ave",
            Phone = "+1-555-0100",
            CreatedAt = DateTime.UtcNow
        };

        context.Offices.Add(office);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
