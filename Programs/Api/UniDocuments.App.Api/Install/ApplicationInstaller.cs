﻿using PhlegmaticOne.JwtTokensGeneration.Extensions;
using PhlegmaticOne.JwtTokensGeneration.Options;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.PasswordHasher.Implementation;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Services.Jwt;

namespace UniDocuments.App.Api.Install;

public static class ApplicationInstaller
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection, IJwtOptions jwtOptions)
    {
        serviceCollection.AddSingleton<IPasswordHasher, SecurePasswordHasher>();
        serviceCollection.AddSingleton<IJwtTokenGenerationService, JwtTokenGenerationService>();
        serviceCollection.AddJwtTokenGeneration(jwtOptions);
        return serviceCollection;
    }
}