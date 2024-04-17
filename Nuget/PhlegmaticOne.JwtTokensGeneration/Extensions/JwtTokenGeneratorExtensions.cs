using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.JwtTokensGeneration.Implementation;
using PhlegmaticOne.JwtTokensGeneration.Options;

namespace PhlegmaticOne.JwtTokensGeneration.Extensions;

public static class JwtTokenGeneratorExtensions
{
    public static IServiceCollection AddJwtTokenGeneration(this IServiceCollection serviceCollection,
        IJwtOptions jwtOptions)
    {
        serviceCollection.AddTransient<IJwtTokenGenerator>(_ => new JwtTokenGenerator(jwtOptions));
        return serviceCollection;
    }
}