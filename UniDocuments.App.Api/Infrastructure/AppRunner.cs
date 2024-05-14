using UniDocuments.App.Api.Infrastructure.Configurations;

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

        app.MapControllers();

        app.Run();
    }
}