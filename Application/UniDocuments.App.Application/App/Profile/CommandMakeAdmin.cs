using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Models.Base;
using UniDocuments.App.Domain.Models.Enums;
using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Application.App.Profile;

public class CommandMakeAdmin : IOperationResultCommand
{
    public string UserName { get; set; } = null!;
    public StudyRole StudyRole { get; set; }
}

public class CommandMakeAdminHandler : IOperationResultCommandHandler<CommandMakeAdmin>
{
    private const string ErrorCode = "UpdateProfile.ProfileNotFound";
    
    private readonly ApplicationDbContext _dbContext;

    public CommandMakeAdminHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult> Handle(CommandMakeAdmin request, CancellationToken cancellationToken)
    {
        return request.StudyRole switch
        {
            StudyRole.Student => await MakeAdmin<Student>(request, cancellationToken),
            StudyRole.Teacher => await MakeAdmin<Teacher>(request, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(request.StudyRole))
        };
    }
    
    private async Task<OperationResult<bool>> MakeAdmin<T>(
        CommandMakeAdmin request, CancellationToken cancellationToken) where T : Person, new()
    {
        var repository = _dbContext.Set<T>();
        var profile = await repository.FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

        if (profile is null)
        {
            return OperationResult.Failed<bool>(ErrorCode);
        }

        profile.Role = ApplicationRole.Admin;
        repository.Update(profile);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult.Successful(true);
    }
}