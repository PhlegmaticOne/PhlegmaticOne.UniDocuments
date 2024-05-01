using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.JwtTokensGeneration.Options;

namespace UniDocuments.App.Api.Install;

public static class AuthenticationInstaller
{
    public static IServiceCollection AddAuthentication(this IServiceCollection serviceCollection, IJwtOptions jwtOptions)
    {
        serviceCollection.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = jwtOptions.GetSecretKey(),
                ClockSkew = TimeSpan.Zero
            };
        });

        return serviceCollection;
    }
}