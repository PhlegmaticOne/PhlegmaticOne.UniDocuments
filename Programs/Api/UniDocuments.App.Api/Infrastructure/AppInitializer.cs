using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.PythonTasks;
using UniDocuments.App.Api.Infrastructure.Configurations;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.Preprocessing.Stopwords;

namespace UniDocuments.App.Api.Infrastructure;

public static class AppInitializer
{
    public static async Task InitializeAsync(WebApplication application, ApplicationConfiguration configuration)
    {
        using var scope = application.Services.CreateScope();
        var services = scope.ServiceProvider;
        var cancellationToken = application.Lifetime.ApplicationStopped;
        
        var stopWordsService = services.GetRequiredService<IStopWordsService>();
        var documentMapperInitializer = services.GetRequiredService<IDocumentMappingInitializer>();
        var pythonTaskPool = services.GetRequiredService<IPythonTaskPool>();
        var passwordHasher = services.GetRequiredService<IPasswordHasher>();
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        var settings = services.GetRequiredService<IOptions<ApplicationSettings>>();
        
        if (configuration.UseRealDatabase)
        {
            var sqlConnectionProvider = services.GetRequiredService<ISqlConnection>();
            await sqlConnectionProvider.InitializeAsync(cancellationToken);
        }

        await CreateOrMigrate(dbContext, cancellationToken);
        await SeedUsersAsync(dbContext, settings.Value, passwordHasher, cancellationToken);
        await stopWordsService.InitializeAsync(cancellationToken);
        await documentMapperInitializer.InitializeAsync(cancellationToken);
        pythonTaskPool.Start(cancellationToken);
    }
    
    private static async Task CreateOrMigrate(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        if (dbContext.Database.IsRelational())
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        else
        {
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        }
    }

    private static async Task SeedUsersAsync(
        ApplicationDbContext context, ApplicationSettings settings, IPasswordHasher passwordHasher, CancellationToken cancellationToken)
    {
        var students = context.Set<Student>();
        var activities = context.Set<StudyActivity>();

        if (await students.AnyAsync(cancellationToken) == false)
        {
            var admin = settings.Admin;
            var adminUser = admin.WithPassword(passwordHasher.Hash(admin.Password));
            await students.AddAsync(adminUser, cancellationToken);
        }

        if (await activities.AnyAsync(cancellationToken) == false)
        {
            var activity = settings.DefaultActivity.ToAnyActivity();
            await activities.AddAsync(activity, cancellationToken);
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }
}