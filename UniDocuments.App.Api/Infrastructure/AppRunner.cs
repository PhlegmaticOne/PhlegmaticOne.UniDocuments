using UniDocuments.App.Api.Infrastructure.Configurations;
using UniDocuments.App.Api.Infrastructure.Roles;

namespace UniDocuments.App.Api.Infrastructure;

public static class AppRunner
{
    public static void Run(WebApplication app, ApplicationConfiguration applicationConfiguration)
    {
        app.UseDeveloperExceptionPage();
        
        app.UseHttpsRedirection();

        if (applicationConfiguration.UseAuthentication)
        {
            app.UseAuthentication();
        }

        app.UseAuthorization();

        app.UseRequireAppRoles();
        app.UseRequireStudyRoles();

        app.MapControllers();

        app.Run();
    }
}