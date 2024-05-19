using UniDocuments.App.Api.Infrastructure.Roles;

namespace UniDocuments.App.Api.Infrastructure;

public static class AppRunner
{
    public static void Run(WebApplication app)
    {
        app.UseDeveloperExceptionPage();
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        
        app.UseAuthorization();

        app.UseRequireAppRoles();
        app.UseRequireStudyRoles();

        app.MapControllers();

        app.Run();
    }
}