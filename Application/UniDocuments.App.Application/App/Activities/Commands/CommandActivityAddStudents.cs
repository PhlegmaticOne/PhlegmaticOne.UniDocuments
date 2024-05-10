using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Activities;
using UniDocuments.App.Shared.Activities.AddStudents;
using UniDocuments.App.Shared.Activities.Display;

namespace UniDocuments.App.Application.App.Activities.Commands;

public class CommandActivityAddStudents : IdentityOperationResultCommand
{
    public ActivityAddStudentsObject AddStudentsObject { get; }

    public CommandActivityAddStudents(Guid profileId, ActivityAddStudentsObject addStudentsObject) : base(profileId)
    {
        AddStudentsObject = addStudentsObject;
    }
}

public class CommandActivityAddStudentsHandler : IOperationResultCommandHandler<CommandActivityAddStudents>
{
    private readonly ApplicationDbContext _dbContext;

    public CommandActivityAddStudentsHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult> Handle(CommandActivityAddStudents request, CancellationToken cancellationToken)
    {
        var addObject = request.AddStudentsObject;
        
        var activities = _dbContext.Set<StudyActivity>();
        var students = _dbContext.Set<Student>();

        var activity = await activities
            .FirstOrDefaultAsync(x => x.Id == addObject.ActivityId, cancellationToken);

        if (activity is null)
        {
            return OperationResult.Failed<ActivityDisplayObject>();
        }
        
        var addStudents = await students
            .Where(x => addObject.Students.Contains(x.Id))
            .ToListAsync(cancellationToken);
        
        activity.Students.AddRange(addStudents);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return OperationResult.Successful(CreateResult(activity));
    }

    private static ActivityDisplayObject CreateResult(StudyActivity activity)
    {
        return new ActivityDisplayObject
        {
            Id = activity.Id,
            StartDate = activity.StartDate,
            EndDate = activity.EndDate,
            Name = activity.Name,
            DocumentsCount = 0,
            StudentsCount = activity.Students.Count
        };
    }
}