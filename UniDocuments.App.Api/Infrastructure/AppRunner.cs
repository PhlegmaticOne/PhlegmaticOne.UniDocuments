using UniDocuments.App.Api.Infrastructure.Configurations;

namespace UniDocuments.App.Api.Infrastructure;

public static class AppRunner
{
    public static void Run(WebApplicationBuilder builder, WebApplication app, ApplicationConfiguration applicationConfiguration)
    {
        // if (builder.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

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