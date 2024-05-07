using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.PythonTasks;
using UniDocuments.App.Api.Infrastructure.Configurations;
using UniDocuments.App.Application;
using UniDocuments.App.Data.EntityFramework.Context;

namespace UniDocuments.App.Api.Infrastructure.Install;

public static class ApplicationRequirementsInstaller
{
    private const string InMemoryDatabase = "MEMORY";
    
    public static IServiceCollection AddApplicationRequirements(
        this IServiceCollection serviceCollection, string connectionString, ApplicationConfiguration applicationConfiguration)
    {
        serviceCollection.AddPythonTaskPool(applicationConfiguration.IncludePythonScripts);
        serviceCollection.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(UniDocumentApplicationReference).Assembly));
        serviceCollection.AddDbContext<ApplicationDbContext>(x =>
        {
            if (!applicationConfiguration.UseRealDatabase)
            {
                x.UseInMemoryDatabase(InMemoryDatabase);
            }
            else
            {
                x.UseSqlServer(connectionString);
            }
        });

        return serviceCollection;
    }
}