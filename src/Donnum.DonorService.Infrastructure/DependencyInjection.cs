using Donnum.BuildingBlocks.Messaging;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Infrastructure.Data;
using Donnum.DonorService.Infrastructure.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Donnum.BuildingBlocks.Messaging.Abstractions;
using Donnum.DonorService.Infrastructure.Messaging;

namespace Donnum.DonorService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDonorRepository, DonorRepository>();
        services.AddScoped<IDonationRepository, DonationRepository>();
        services.AddScoped<IBadgeRepository, BadgeRepository>();

        services.AddMessageBroker(configuration);
        services.AddScoped<IIntegrationEventOutbox, IntegrationEventOutbox>();
        services.AddHostedService<OutboxPublisherWorker>();
        services.AddHostedService<BloodRequestCreatedConsumer>();

        return services;
    }
}
