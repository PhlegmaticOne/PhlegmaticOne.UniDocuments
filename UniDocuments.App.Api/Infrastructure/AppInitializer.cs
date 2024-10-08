﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.PythonTasks;
using UniDocuments.App.Api.Infrastructure.Configurations;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Services.Common;
using UniDocuments.Text.Domain.Services.DocumentMapping;
using UniDocuments.Text.Domain.Services.Preprocessing.Stopwords;

namespace UniDocuments.App.Api.Infrastructure;

public static class AppInitializer
{
    public static async Task<WebApplication> InitializeAsync(this WebApplication application)
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
        var timeProvider = services.GetRequiredService<ITimeProvider>();

        await CreateOrMigrate(dbContext, cancellationToken);
        await DatabaseSeed.SeedAsync(dbContext, settings.Value, timeProvider, passwordHasher, cancellationToken);
        await stopWordsService.InitializeAsync(cancellationToken);
        await documentMapperInitializer.InitializeAsync(cancellationToken);
        pythonTaskPool.Start(cancellationToken);
        PythonTask.TaskPool = pythonTaskPool;

        return application;
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
}

public static class DatabaseSeed
{
    public static async Task SeedAsync(
        ApplicationDbContext context, ApplicationSettings settings,
        ITimeProvider timeProvider, IPasswordHasher passwordHasher, CancellationToken cancellationToken)
    {
        var students = context.Set<Student>();
        var activities = context.Set<StudyActivity>();
        var teachers = context.Set<Teacher>();
        var date = timeProvider.Now;
        Teacher? teacher = null;
        Student? student = null;

        if (await students.AnyAsync(cancellationToken) == false)
        {
            var users = new List<Student>
            {
                settings.Admin.With<Student>(
                    passwordHasher.Hash(settings.Admin.Password), date),
            };

            student = users[0];
            await students.AddRangeAsync(users, cancellationToken);
        }

        if (await teachers.AnyAsync(cancellationToken) == false)
        {
            teacher = settings.Teacher.With<Teacher>(
                passwordHasher.Hash(settings.Teacher.Password), date);
            await teachers.AddAsync(teacher, cancellationToken);
        }

        if (await activities.AnyAsync(cancellationToken) == false)
        {
            var activity = settings.DefaultActivity.ToAnyActivity(teacher!, date);
            activity.Students.Add(student!);
            await activities.AddAsync(activity, cancellationToken);
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }
}