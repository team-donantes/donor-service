using System.Reflection;
using Donnum.DonorService.Application.Behaviors;
using Donnum.DonorService.Application.Features.Donors.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Donnum.DonorService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddScoped<IEvaluateAndAssignBadgesService, EvaluateAndAssignBadgesService>();

        return services;
    }
}
