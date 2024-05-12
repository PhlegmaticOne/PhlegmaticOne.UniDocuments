using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PagedLists.Extensions;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Activities.My;
using UniDocuments.App.Shared.Activities.Shared;
using UniDocuments.App.Shared.Shared;

namespace UniDocuments.App.Application.App.Activities.Queries;

public class QueryGetActivitiesStudent : IdentityOperationResultQuery<ActivityMyList>
{
    public PagedListData Data { get; }
    
    public QueryGetActivitiesStudent(Guid profileId, PagedListData data) : base(profileId)
    {
        Data = data;
    }
}

public class QueryGetActivitiesStudentHandler : IOperationResultQueryHandler<QueryGetActivitiesStudent, ActivityMyList>
{
    private readonly ApplicationDbContext _dbContext;

    public QueryGetActivitiesStudentHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult<ActivityMyList>> Handle(
        QueryGetActivitiesStudent request, CancellationToken cancellationToken)
    {
        var activities = await _dbContext.Set<StudyActivity>()
            .Where(x => x.Students.Any(s => s.Id == request.ProfileId))
            .OrderByDescending(x => x.StartDate)
            .Select(x => new ActivityMyObject
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Name = x.Name,
                CreatorFirstName = x.Creator.FirstName,
                CreatorLastName = x.Creator.LastName,
                Description = x.Description,
                Document = x.Documents
                    .Where(d => d.StudentId == request.ProfileId)
                    .Select(d => new ActivityDocumentObject 
                    { 
                        Id = d.Id, 
                        Name = d.Name, 
                        DateLoaded = d.DateLoaded 
                    }).FirstOrDefault()
            })
            .ToPagedListAsync(request.Data.PageIndex, request.Data.PageSize, cancellationToken: cancellationToken);

        return OperationResult.Successful(new ActivityMyList
        {
            Activities = activities
        });
    }
}