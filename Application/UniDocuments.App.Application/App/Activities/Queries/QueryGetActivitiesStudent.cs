using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Activities;
using UniDocuments.App.Shared.Activities.Display;

namespace UniDocuments.App.Application.App.Activities.Queries;

public class QueryGetActivitiesStudent : IdentityOperationResultQuery<ActivityDisplayList>
{
    public QueryGetActivitiesStudent(Guid profileId) : base(profileId) { }
}

public class QueryGetActivitiesStudentHandler : IOperationResultQueryHandler<QueryGetActivitiesTeacher, ActivityDisplayList>
{
    private readonly ApplicationDbContext _dbContext;

    public QueryGetActivitiesStudentHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult<ActivityDisplayList>> Handle(
        QueryGetActivitiesTeacher request, CancellationToken cancellationToken)
    {
        var activities = await _dbContext.Set<StudyActivity>()
            .Where(x => x.Students.Any(s => s.Id == request.ProfileId))
            .Select(x => new ActivityDisplayObject
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                DocumentsCount = x.Documents.Count,
                StudentsCount = x.Students.Count,
                Name = x.Name,
                Creator = x.Creator.UserName,
                IsExpired = DateTime.UtcNow > x.EndDate
            }).ToListAsync(cancellationToken);

        return OperationResult.Successful(new ActivityDisplayList
        {
            Activities = activities
        });
    }
}