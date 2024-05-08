using UniDocuments.App.Api.Infrastructure.Configurations;

namespace UniDocuments.App.Api.Infrastructure.Install;

public static class ApplicationConfigurationsInstaller
{
    public static IServiceCollection AddApplicationConfiguration(
        this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<ApplicationConfiguration>(configuration.GetSection(nameof(ApplicationConfiguration)));
        serviceCollection.Configure<ApplicationSettings>(configuration.GetSection(nameof(ApplicationSettings)));
        return serviceCollection;
    }
}