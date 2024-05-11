using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Shared.Activities;
using UniDocuments.App.Shared.Activities.Create;
using UniDocuments.App.Shared.Activities.Detailed;
using UniDocuments.App.Shared.Activities.Display;

namespace UniDocuments.App.Application.App.Activities.Commands;

public class CommandCreateActivity : IdentityOperationResultCommand
{
    public ActivityCreateObject CreateActivityObject { get; }

    public CommandCreateActivity(Guid profileId, ActivityCreateObject createActivityObject) : base(profileId)
    {
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
            var found = students.Select(x => x.UserName.ToLower());
            var notFound = createObject.Students.Except(found).ToList();
            return OperationResult.Failed(result: notFound);
        }

        var entry = await _dbContext.Set<StudyActivity>().AddAsync(new StudyActivity
        {
            Name = createObject.Name,
            Description = createObject.Description,
            StartDate = createObject.StartDate,
            EndDate = createObject.EndDate,
            CreatorId = request.ProfileId,
            Students = students,
            CreationDate = _timeProvider.Now,
        }, cancellationToken);

        var creatorName = await _dbContext.Set<Teacher>()
            .Where(x => x.Id == request.ProfileId)
            .Select(x => new
            {
                x.FirstName,
                x.LastName
            })
            .FirstOrDefaultAsync(cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return OperationResult.Successful(new ActivityDetailedObject
        {
            Id = entry.Entity.Id,
            Name = createObject.Name,
            StartDate = createObject.StartDate,
            EndDate = createObject.EndDate,
            CreatorFirstName = creatorName!.FirstName,
            CreatorLastName = creatorName.LastName,
            Description = createObject.Description,
        });
    }
}