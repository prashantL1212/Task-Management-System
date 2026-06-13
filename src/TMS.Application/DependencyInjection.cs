using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TMS.Application.Interfaces.Services;
using TMS.Application.Services;

namespace TMS.Application;

/// <summary>
/// Registers the Application layer's services with the DI container. Called from
/// the API composition root via <c>builder.Services.AddApplication()</c>.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register all FluentValidation validators in this assembly.
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Business services.
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
