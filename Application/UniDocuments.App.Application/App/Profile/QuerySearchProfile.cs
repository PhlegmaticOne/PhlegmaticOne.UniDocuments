using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Application.App.Profile;

public class QuerySearchProfile : IdentityOperationResultQuery<ProfileSearchObject>
{
    public string SearchName { get; }

    public QuerySearchProfile(Guid profileId, string searchName) : base(profileId)
    {
        SearchName = searchName;
    }
}

public class QuerySearchProfileHandler : IOperationResultQueryHandler<QuerySearchProfile, ProfileSearchObject>
{
    private readonly ApplicationDbContext _dbContext;

    public QuerySearchProfileHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult<ProfileSearchObject>> Handle(
        QuerySearchProfile request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Set<Student>()
            .Where(x => x.UserName.ToLower() == request.SearchName)
            .Select(x => new ProfileSearchObject
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                Id = x.Id
            })
            .FirstOrDefaultAsync(cancellationToken);
        
        return OperationResult.Successful(result!);
    }
}