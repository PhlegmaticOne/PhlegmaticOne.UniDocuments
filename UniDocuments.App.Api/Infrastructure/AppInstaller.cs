using UniDocuments.App.Api.Infrastructure.Configurations;
using UniDocuments.App.Api.Infrastructure.Install;

namespace UniDocuments.App.Api.Infrastructure;

public class AppInstaller
{
    private const string ConnectionStringName = "DbConnection";
    
    public static ApplicationConfiguration Install(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(ConnectionStringName)!;
        var applicationConfiguration = GetSection<ApplicationConfiguration>(builder.Configuration)!;
        var jwtOptions = applicationConfiguration.CreateJwtOptions();

        builder.Services.AddApplicationWeb(applicationConfiguration, jwtOptions);
        builder.Services.AddApplicationConfiguration(builder.Configuration);
        builder.Services.AddApplicationServices(jwtOptions);
        builder.Services.AddDocumentApplication(builder.Configuration, connectionString, applicationConfiguration);
        builder.Services.AddApplicationRequirements(connectionString, applicationConfiguration);
        
        return applicationConfiguration;
    }

    private static T GetSection<T>(IConfiguration configuration)
    {
        return configuration.GetSection(typeof(T).Name).Get<T>()!;
    } 
}