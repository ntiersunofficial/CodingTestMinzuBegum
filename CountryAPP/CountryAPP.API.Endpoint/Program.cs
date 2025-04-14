using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;
using CountryAPP.API.Endpoint;
using CountryAPP.API.Endpoint.Exceptions;
using CountryAPP.Core.Constant;
using CountryAPP.API.Persistence.Identity;
using CountryAPP.API.Persistence;
using CountryAPP.Core.Contract.Persistence;
using CountryAPP.Core.Contract.Infrastructure;
using CountryAPP.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Register services
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new(1, 0);
    options.ReportApiVersions = true;
});

// IMPORTANT: Add this NuGet package for the following to work:
// <PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer" Version="5.0.0" />
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CountryAPP.API.Endpoint v1",
        Description = "Web API for CountryAPP.",
        TermsOfService = new Uri("https://localhost/terms"),
        License = new OpenApiLicense { Name = "MIT" },
        Contact = new OpenApiContact
        {
            Name = "CountryAPP Helpdesk",
            Email = "help@CountryAPP.com",
            Url = new Uri("https://www.CountryAPP.com")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }},
            new string[] {}
        }
    });
});

// PostgreSQL EF Core Context
builder.Services.AddDbContext<CountryDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT:Issuer"],
            ValidAudience = configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]))
        };
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Constants.SystemAdmin, policy =>
        policy.RequireClaim("UserRole", "SystemAdmin"));

    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("LimiterPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = 4;
        limiterOptions.Window = TimeSpan.FromSeconds(10);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });
});

builder.Services.AddScoped<IDataAccessHelper, DataAccessHelper>();
builder.Services.AddScoped<ISecurityHelper, SecurityHelper>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddHttpClient<ICountryRepository, CountryRepository>();

var app = builder.Build();

// Middleware pipeline
app.UseDeveloperExceptionPage(); // You might conditionally disable this in production
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CountryAPP.API.Endpoint v1");
});

// Enable HTTPS, routing, CORS, auth, rate limiting
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

// Exception middleware (custom)
app.ConfigureBuiltInExceptionHandler();

// Redirect root ("/") to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger")).AllowAnonymous();

// Controllers
app.MapControllers();

app.Run();
