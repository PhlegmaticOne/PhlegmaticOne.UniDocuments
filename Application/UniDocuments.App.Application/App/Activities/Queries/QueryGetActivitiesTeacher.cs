using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Activities;
using UniDocuments.App.Shared.Activities.Display;

namespace UniDocuments.App.Application.App.Activities.Queries;

public class QueryGetActivitiesTeacher : IdentityOperationResultQuery<ActivityDisplayList>
{
    public QueryGetActivitiesTeacher(Guid profileId) : base(profileId) { }
}

public class QueryGetActivitiesTeacherHandler : IOperationResultQueryHandler<QueryGetActivitiesTeacher, ActivityDisplayList>
{
    private readonly ApplicationDbContext _dbContext;

    public QueryGetActivitiesTeacherHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult<ActivityDisplayList>> Handle(
        QueryGetActivitiesTeacher request, CancellationToken cancellationToken)
    {
        var activities = await _dbContext.Set<StudyActivity>()
            .Where(x => x.CreatorId == request.ProfileId)
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