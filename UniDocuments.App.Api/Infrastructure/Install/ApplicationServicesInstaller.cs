using PhlegmaticOne.JwtTokensGeneration.Extensions;
using PhlegmaticOne.JwtTokensGeneration.Options;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.PasswordHasher.Implementation;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Domain.Services.Documents;
using UniDocuments.App.Services;
using UniDocuments.App.Services.Documents;

namespace UniDocuments.App.Api.Infrastructure.Install;

public static class ApplicationServicesInstaller
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection, IJwtOptions jwtOptions)
    {
        serviceCollection.AddSingleton<IPasswordHasher, SecurePasswordHasher>();
        serviceCollection.AddSingleton<IJwtTokenGenerationService, JwtTokenGenerationService>();
        serviceCollection.AddSingleton<IProfileSetuper, ProfileSetuper>();
        serviceCollection.AddSingleton<ITimeProvider, TimeProvider>();
        serviceCollection.AddScoped<IDocumentSaveProvider, DocumentSaveProvider>();
        serviceCollection.AddJwtTokenGeneration(jwtOptions);
        return serviceCollection;
    }
}