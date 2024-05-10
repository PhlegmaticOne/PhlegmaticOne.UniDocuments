using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Activities;
using UniDocuments.App.Shared.Activities.Create;
using UniDocuments.App.Shared.Activities.Display;

namespace UniDocuments.App.Application.App.Activities.Commands;

public class CommandCreateActivity : IdentityOperationResultCommand
{
    public CreateActivityObject CreateActivityObject { get; }

    public CommandCreateActivity(Guid profileId, CreateActivityObject createActivityObject) : base(profileId)
    {
        CreateActivityObject = createActivityObject;
    }
}

public class CommandCreateActivityHandler : IOperationResultCommandHandler<CommandCreateActivity>
{
    private readonly ApplicationDbContext _dbContext;

    public CommandCreateActivityHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult> Handle(CommandCreateActivity request, CancellationToken cancellationToken)
    {
        var createObject = request.CreateActivityObject;
        var activities = _dbContext.Set<StudyActivity>();

        var entry = await activities.AddAsync(new StudyActivity
        {
            Name = createObject.Name,
            Description = createObject.Description,
            StartDate = createObject.StartDate,
            EndDate = createObject.EndDate,
            CreatorId = createObject.TeacherId,
        }, cancellationToken);

        var creatorName = await _dbContext.Set<Teacher>()
            .Where(x => x.Id == createObject.TeacherId)
            .Select(x => x.UserName)
            .FirstOrDefaultAsync(cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        var result = new ActivityDisplayObject
        {
            Id = entry.Entity.Id,
            Name = createObject.Name,
            StartDate = createObject.StartDate,
            EndDate = createObject.EndDate,
            Creator = creatorName!,
            DocumentsCount = 0,
            StudentsCount = 0,
            IsExpired = DateTime.UtcNow > createObject.EndDate
        };
        
        return OperationResult.Successful(result);
    }
}