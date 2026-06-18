using Donnum.BuildingBlocks.Messaging;
using Donnum.DonorService.Domain.Repositories;
using Donnum.DonorService.Infrastructure.Data;
using Donnum.DonorService.Infrastructure.Data.Repositories;
using Donnum.DonorService.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Donnum.DonorService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDonorRepository, DonorRepository>();
        services.AddScoped<IDonationRepository, DonationRepository>();

        services.AddMessageBroker(configuration);

        services.Configure<DonationCompletedEventSubscriptionOptions>(
            configuration.GetSection(DonationCompletedEventSubscriptionOptions.SectionName));
        services.AddSingleton<DonationCompletedMessageConsumer>();
        services.AddHostedService(provider => provider.GetRequiredService<DonationCompletedMessageConsumer>());

        return services;
    }
}
