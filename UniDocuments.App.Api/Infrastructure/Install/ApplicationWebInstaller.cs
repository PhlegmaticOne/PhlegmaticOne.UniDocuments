using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.JwtTokensGeneration.Options;
using UniDocuments.App.Api.Infrastructure.Configurations;

namespace UniDocuments.App.Api.Infrastructure.Install;

public static class ApplicationWebInstaller
{
    public static IServiceCollection AddApplicationWeb(
        this IServiceCollection serviceCollection, ApplicationConfiguration configuration, IJwtOptions jwtOptions)
    {
        if (configuration.UseAuthentication)
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
        }

        serviceCollection.AddControllers().AddNewtonsoftJson();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();
        return serviceCollection;
    }
}