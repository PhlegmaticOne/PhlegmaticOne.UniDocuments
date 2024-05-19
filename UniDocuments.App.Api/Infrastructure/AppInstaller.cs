using UniDocuments.App.Api.Infrastructure.Configurations;
using UniDocuments.App.Api.Infrastructure.Install;

namespace UniDocuments.App.Api.Infrastructure;

public static class AppInstaller
{
    private const string ConnectionStringName = "DbConnection";
    
    public static WebApplicationBuilder InstallRequirements(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(ConnectionStringName)!;
        var applicationConfiguration = GetSection<ApplicationConfiguration>(builder.Configuration)!;
        var jwtOptions = applicationConfiguration.CreateJwtOptions();

        builder.Services.AddApplicationWeb(jwtOptions);
        builder.Services.AddApplicationConfiguration(builder.Configuration);
        builder.Services.AddApplicationServices(jwtOptions);
        builder.Services.AddDocumentApplication(builder.Configuration, applicationConfiguration);
        builder.Services.AddApplicationRequirements(connectionString, applicationConfiguration);
        return builder;
    }

    private static T GetSection<T>(IConfiguration configuration)
    {
        return configuration.GetSection(typeof(T).Name).Get<T>()!;
    } 
}