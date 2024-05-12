using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PagedLists.Extensions;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Activities.Created;
using UniDocuments.App.Shared.Shared;

namespace UniDocuments.App.Application.App.Activities.Queries;

public class QueryGetActivitiesTeacher : IOperationResultQuery<ActivityCreatedList>
{
    public IdentityProfileData ProfileData { get; }
    public PagedListData Data { get; }

    public QueryGetActivitiesTeacher(IdentityProfileData profileData, PagedListData data)
    {
        ProfileData = profileData;
        Data = data;
    }
}

public class QueryGetActivitiesTeacherHandler : IOperationResultQueryHandler<QueryGetActivitiesTeacher, ActivityCreatedList>
{
    private readonly ApplicationDbContext _dbContext;

    public QueryGetActivitiesTeacherHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult<ActivityCreatedList>> Handle(
        QueryGetActivitiesTeacher request, CancellationToken cancellationToken)
    {
        var activities = await _dbContext.Set<StudyActivity>()
            .Where(x => x.CreatorId == request.ProfileData.Id)
            .OrderByDescending(x => x.StartDate)
            .Select(x => new ActivityCreatedObject
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                DocumentsCount = x.Documents.Count,
                StudentsCount = x.Students.Count,
                Name = x.Name,
                CreatorFirstName = request.ProfileData.FirstName,
                CreatorLastName = request.ProfileData.LastName,
            })
            .ToPagedListAsync(request.Data.PageIndex, request.Data.PageSize, cancellationToken: cancellationToken);

        return OperationResult.Successful(new ActivityCreatedList
        {
            Activities = activities
        });
    }
}