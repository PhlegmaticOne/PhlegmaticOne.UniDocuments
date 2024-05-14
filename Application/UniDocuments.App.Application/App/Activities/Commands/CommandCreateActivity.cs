using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Services.Common;
using UniDocuments.App.Shared.Activities.Create;
using UniDocuments.App.Shared.Activities.Detailed;

namespace UniDocuments.App.Application.App.Activities.Commands;

public class CommandCreateActivity : IOperationResultCommand
{
    public IdentityProfileData ProfileData { get; }
    public ActivityCreateObject CreateActivityObject { get; }

    public CommandCreateActivity(IdentityProfileData profileData, ActivityCreateObject createActivityObject)
    {
        ProfileData = profileData;
        CreateActivityObject = createActivityObject;
    }
}

public class CommandCreateActivityHandler : IOperationResultCommandHandler<CommandCreateActivity>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ITimeProvider _timeProvider;

    public CommandCreateActivityHandler(ApplicationDbContext dbContext, ITimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _timeProvider = timeProvider;
    }
    
    public async Task<OperationResult> Handle(CommandCreateActivity request, CancellationToken cancellationToken)
    {
        var createObject = request.CreateActivityObject;

        var students = await _dbContext.Set<Student>()
            .Where(x => createObject.Students.Contains(x.UserName.ToLower()))
            .ToListAsync(cancellationToken);

        if (students.Count != createObject.Students.Count)
        {
            return ErrorWithNotFoundStudents(students, createObject);
        }

        var entity = await CreateActivity(request.ProfileData.Id, createObject, students, cancellationToken);
        return OperationResult.Successful(CreateResult(entity, request));
    }

    private OperationResult<ActivityDetailedObject> ErrorWithNotFoundStudents(
        List<Student> students, ActivityCreateObject createObject)
    {
        var found = students.Select(x => x.UserName.ToLower());
        var notFound = createObject.Students.Except(found).ToList();
        var errorData = JsonConvert.SerializeObject(notFound);
        return OperationResult.Failed<ActivityDetailedObject>(errorData: errorData);
    }
    
    private async Task<StudyActivity> CreateActivity(
        Guid profileId, ActivityCreateObject createObject, List<Student> students, CancellationToken cancellationToken)
    {
        var entry = await _dbContext.Set<StudyActivity>().AddAsync(new StudyActivity
        {
            Name = createObject.Name,
            Description = createObject.Description,
            StartDate = createObject.StartDate,
            EndDate = createObject.EndDate,
            Students = students,
            CreationDate = _timeProvider.Now,
            CreatorId = profileId
        }, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    private static ActivityDetailedObject CreateResult(StudyActivity entity, CommandCreateActivity request)
    {
        return new ActivityDetailedObject
        {
            Id = entity.Id,
            Name = entity.Name,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            CreatorFirstName = request.ProfileData.FirstName,
            CreatorLastName = request.ProfileData.LastName,
            Description = entity.Description,
            CreationDate = entity.CreationDate,
            Students = entity.Students.Select(x => new ActivityDetailedStudentObject
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Document = null
            }).ToList()
        };
    }
}