using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Users;
using StudyRole = UniDocuments.App.Shared.Users.Enums.StudyRole;

namespace UniDocuments.App.Application.Profile;

public class QueryGetDetailedProfile : IdentityOperationResultQuery<DetailedProfileObject>
{
    public QueryGetDetailedProfile(Guid profileId) : base(profileId) { }
}

public class QueryGetDetailedProfileHandler : IOperationResultQueryHandler<QueryGetDetailedProfile, DetailedProfileObject>
{
    private const string ErrorCode = "GetDetailedProfile.ProfileNotFound";
    
    private readonly ApplicationDbContext _dbContext;

    public QueryGetDetailedProfileHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OperationResult<DetailedProfileObject>> Handle(
        QueryGetDetailedProfile request, CancellationToken cancellationToken)
    {
        var profile = await _dbContext.Set<Student>()
            .Where(x => x.Id == request.ProfileId)
            .Select(x => new DetailedProfileObject
            {
                Id = x.Id,
                Role = (StudyRole)x.Role,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
            })
            .FirstOrDefaultAsync(cancellationToken);
        
        return profile is null ? 
            OperationResult.Failed<DetailedProfileObject>(ErrorCode) :
            OperationResult.Successful(profile);
    }
}