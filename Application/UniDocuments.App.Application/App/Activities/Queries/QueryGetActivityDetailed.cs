using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Activities.Detailed;

namespace UniDocuments.App.Application.App.Activities.Queries;

public class QueryGetActivityDetailed : IdentityOperationResultQuery<ActivityDetailedObject>
{
    public Guid ActivityId { get; }

    public QueryGetActivityDetailed(Guid profileId, Guid activityId) : base(profileId)
    {
        ActivityId = activityId;
    }
}

public class QueryGetActivityDetailedHandler :
    IOperationResultQueryHandler<QueryGetActivityDetailed, ActivityDetailedObject>
{
    private readonly ApplicationDbContext _dbContext;

    public QueryGetActivityDetailedHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult<ActivityDetailedObject>> Handle(
        QueryGetActivityDetailed request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<StudyActivity>()
            .Where(x => x.Id == request.ActivityId)
            .Select(x => new ActivityDetailedObject
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CreatorFirstName = x.Creator.FirstName,
                CreatorLastName = x.Creator.LastName,
                Students = x.Students.Select(s => new ActivityDetailedStudentObject
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                }).ToList(),
                Documents = x.Documents.Select(d => new ActivityDetailedDocumentObject
                {
                    Id = d.Id,
                    Name = d.Name,
                    DateLoaded = d.DateLoaded,
                    StudentId = d.StudentId
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
        
        return OperationResult.Successful(result!);
    }
}