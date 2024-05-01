using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.PythonTasks;
using UniDocuments.App.Application;
using UniDocuments.App.Data.EntityFramework.Context;

namespace UniDocuments.App.Api.Install;

public static class ApplicationRequirementsInstaller
{
    public static IServiceCollection AddApplicationRequirements(
        this IServiceCollection serviceCollection, string connectionString, ApplicationConfiguration applicationConfiguration)
    {
        serviceCollection.AddPythonTaskPool("keras2vec", "doc2vec");
        serviceCollection.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(UniDocumentApplicationReference).Assembly));
        serviceCollection.AddDbContext<ApplicationDbContext>(x =>
        {
            if (!applicationConfiguration.UseRealDatabase)
            {
                x.UseInMemoryDatabase("MEMORY");
            }
            else
            {
                x.UseSqlServer(connectionString);
            }
        });

        return serviceCollection;
    }
}