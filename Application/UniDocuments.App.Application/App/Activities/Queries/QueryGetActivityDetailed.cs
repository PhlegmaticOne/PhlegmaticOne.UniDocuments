using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Activities.Detailed;
using UniDocuments.App.Shared.Activities.Shared;

namespace UniDocuments.App.Application.App.Activities.Queries;

public class QueryGetActivityDetailed : IOperationResultQuery<ActivityDetailedObject>
{
    public IdentityProfileData ProfileData { get; }
    public Guid ActivityId { get; }

    public QueryGetActivityDetailed(IdentityProfileData profileData, Guid activityId)
    {
        ProfileData = profileData;
        ActivityId = activityId;
    }
}

public class QueryGetActivityDetailedHandler :
    IOperationResultQueryHandler<QueryGetActivityDetailed, ActivityDetailedObject>
{
    private const string ErrorActivityNotFound = "GetActivityDetailed.NotFound";
    
    private class ActivityDetailedPrivate
    {
        public Guid Id { get; set; }
        public string CreatorFirstName { get; set; } = null!;
        public string CreatorLastName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public List<ActivityDetailedStudentPrivate> Students { get; set; } = new();
        public List<ActivityDetailedDocumentPrivate> Documents { get; set; } = new();
    }
    
    private class ActivityDetailedStudentPrivate
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    
    private class ActivityDetailedDocumentPrivate
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateLoaded { get; set; }
    }
    
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
            .Select(x => new ActivityDetailedPrivate
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CreatorFirstName = request.ProfileData.FirstName,
                CreatorLastName = request.ProfileData.LastName,
                CreationDate = x.CreationDate,
                Students = x.Students.Select(s => new ActivityDetailedStudentPrivate
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName
                }).ToList(),
                Documents = x.Documents.Select(d => new ActivityDetailedDocumentPrivate
                {
                    Id = d.Id,
                    Name = d.Name,
                    DateLoaded = d.DateLoaded,
                    StudentId = d.StudentId
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        return result is null ? 
            OperationResult.Failed<ActivityDetailedObject>(ErrorActivityNotFound) : 
            OperationResult.Successful(Map(result));
    }

    private static ActivityDetailedObject Map(ActivityDetailedPrivate selectedObject)
    {
        var query =
            from student in selectedObject.Students
            join document in selectedObject.Documents on student.Id equals document.StudentId into groupJoin
            from subgroup in groupJoin.DefaultIfEmpty()
            select new ActivityDetailedStudentObject
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Document = subgroup is null ? null : new ActivityDocumentObject
                {
                    Id = subgroup.Id,
                    Name = subgroup.Name,
                    DateLoaded = subgroup.DateLoaded
                }
            };

        return new ActivityDetailedObject
        {
            Id = selectedObject.Id,
            Description = selectedObject.Description,
            Name = selectedObject.Name,
            StartDate = selectedObject.StartDate,
            EndDate = selectedObject.EndDate,
            CreatorFirstName = selectedObject.CreatorFirstName,
            CreatorLastName = selectedObject.CreatorLastName,
            CreationDate = selectedObject.CreationDate,
            Students = query.ToList()
        };
    }
}