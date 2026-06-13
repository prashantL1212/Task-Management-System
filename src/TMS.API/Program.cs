using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.API.Extensions;
using TMS.API.Middleware;
using TMS.Application;
using TMS.Application.Interfaces.Security;
using TMS.Infrastructure;
using TMS.Infrastructure.Persistence;
using TMS.Infrastructure.Seed;
using TMS.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC controllers + standardized ApiResponse for validation (400) failures.
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(entry => entry.Value?.Errors.Count > 0)
                .SelectMany(entry => entry.Value!.Errors.Select(e => e.ErrorMessage))
                .ToList();

            return new BadRequestObjectResult(
                ApiResponse.Failure<object>("Validation failed.", errors));
        };
    });

// FluentValidation auto-validation (validators are registered by AddApplication).
builder.Services.AddFluentValidationAutoValidation();

// Application + Infrastructure layers.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Authentication / authorization.
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// Swagger with JWT support.
builder.Services.AddSwaggerWithJwt();

var app = builder.Build();

// Apply EF Core migrations on startup (all environments); seed only in Development.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    if (app.Environment.IsDevelopment())
    {
        var passwordHasher = services.GetRequiredService<IPasswordHasher>();
        await DevelopmentDataSeeder.SeedAsync(dbContext, passwordHasher);
    }
}

// Middleware pipeline (exception handling first so it wraps everything).
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
