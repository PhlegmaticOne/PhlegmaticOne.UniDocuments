using PhlegmaticOne.JwtTokensGeneration.Extensions;
using PhlegmaticOne.JwtTokensGeneration.Options;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.PasswordHasher.Implementation;
using UniDocuments.App.Domain.Services.Common;
using UniDocuments.App.Domain.Services.Documents;
using UniDocuments.App.Domain.Services.Profiles;
using UniDocuments.App.Services.Common;
using UniDocuments.App.Services.Documents;
using UniDocuments.App.Services.Profiles;

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