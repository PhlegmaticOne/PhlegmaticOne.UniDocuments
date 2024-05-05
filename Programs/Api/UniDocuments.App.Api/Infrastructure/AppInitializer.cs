using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.PythonTasks;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.DocumentsStorage;
using UniDocuments.Text.Domain.Services.Preprocessing;

namespace UniDocuments.App.Api.Infrastructure;

public static class AppInitializer
{
    public static async Task InitializeAsync(WebApplication application, ApplicationConfiguration configuration, CancellationToken cancellationToken)
    {
        using var scope = application.Services.CreateScope();
        var services = scope.ServiceProvider;
        
        var stopWordsService = services.GetRequiredService<IStopWordsService>();
        var documentMapperInitializer = services.GetRequiredService<IDocumentMappingInitializer>();
        var pythonTaskPool = services.GetRequiredService<IPythonTaskPool>();
        var passwordHasher = services.GetRequiredService<IPasswordHasher>();
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        
        if (configuration.UseRealDatabase)
        {
            var sqlConnectionProvider = services.GetRequiredService<ISqlConnectionProvider>();
            await sqlConnectionProvider.InitializeAsync(cancellationToken);
        }

        await CreateOrMigrate(dbContext, cancellationToken);
        await SeedUsersAsync(dbContext, passwordHasher, cancellationToken);
        await stopWordsService.InitializeAsync(cancellationToken);
        await documentMapperInitializer.InitializeAsync(cancellationToken);
        pythonTaskPool.Start(cancellationToken);
    }
    
    private static async Task CreateOrMigrate(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        if (dbContext.Database.IsRelational())
        {
            await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);
        }
        else
        {
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        }
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context, IPasswordHasher passwordHasher, CancellationToken cancellationToken)
    {
        var students = context.Set<Student>();
        var activities = context.Set<StudyActivity>();

        if (await students.AnyAsync(cancellationToken: cancellationToken) == false)
        {
            var student = new Student
            {
                Password = passwordHasher.Hash("Qwerty_1234"),
                FirstName = "Александр",
                LastName = "Кротов",
                UserName = "aleksandr_krotov"
            };

            await students.AddAsync(student, cancellationToken);
        }

        if (await activities.AnyAsync(cancellationToken) == false)
        {
            var activity = new StudyActivity
            {
                Description = "Активность для всех документов",
                Name = "Any",
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MaxValue
            };

            await activities.AddAsync(activity, cancellationToken);
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }
}