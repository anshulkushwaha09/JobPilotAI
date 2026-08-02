using JobPilot.API.Configurations;
using JobPilot.API.Data;
using JobPilot.API.Helpers;
using JobPilot.API.Repositories.Implementations;
using JobPilot.API.Repositories.Interfaces;
using JobPilot.API.Services.Email.Implementations;
using JobPilot.API.Services.Email.Interfaces;
using JobPilot.API.Services.Implementations;
using JobPilot.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Services

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter JWT Token"
        });

    options.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type =
                                Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,

                            Id="Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

// Database
builder.Services.AddSingleton<DbConnectionFactory>();

// JWT Configuration
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

JwtSettings jwtSettings =
    builder.Configuration
           .GetSection("Jwt")
           .Get<JwtSettings>()!;

// Single Authentication and JWT Bearer Configuration
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
});

builder.Services.AddAuthorization();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email"));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<GoogleAuthSettings>(
builder.Configuration.GetSection("GoogleAuth"));
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<JwtTokenHelper>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IResumeRepository, ResumeRepository>();

builder.Services.AddScoped<IResumeService, ResumeService>();

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.Configure<FileStorageSettings>(
    builder.Configuration.GetSection("FileStorage"));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

#endregion

var app = builder.Build();

#region Middleware

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();