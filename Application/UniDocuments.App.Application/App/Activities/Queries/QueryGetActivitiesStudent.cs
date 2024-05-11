﻿using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PagedLists.Extensions;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Activities.Display;
using UniDocuments.App.Shared.Shared;

namespace UniDocuments.App.Application.App.Activities.Queries;

public class QueryGetActivitiesStudent : IdentityOperationResultQuery<ActivityDisplayList>
{
    public PagedListData Data { get; }
    
    public QueryGetActivitiesStudent(Guid profileId, PagedListData data) : base(profileId)
    {
        Data = data;
    }
}

public class QueryGetActivitiesStudentHandler : IOperationResultQueryHandler<QueryGetActivitiesStudent, ActivityDisplayList>
{
    private readonly ApplicationDbContext _dbContext;

    public QueryGetActivitiesStudentHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult<ActivityDisplayList>> Handle(
        QueryGetActivitiesStudent request, CancellationToken cancellationToken)
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
                CreatorFirstName = x.Creator.FirstName,
                CreatorLastName = x.Creator.LastName,
                IsExpired = DateTime.UtcNow > x.EndDate
            }).ToPagedListAsync(request.Data.PageIndex, request.Data.PageSize, cancellationToken: cancellationToken);

        return OperationResult.Successful(new ActivityDisplayList
        {
            Activities = activities
        });
    }
}