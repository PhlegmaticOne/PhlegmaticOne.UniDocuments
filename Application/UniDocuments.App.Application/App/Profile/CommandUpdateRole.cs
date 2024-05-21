using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Models.Base;
using UniDocuments.App.Domain.Models.Enums;
using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Application.App.Profile;

public class CommandUpdateRole : IOperationResultCommand
{
    public string UserName { get; set; } = null!;
    public AppRole AppRole { get; set; }
    public StudyRole StudyRole { get; set; }
}

public class CommandUpdateRoleHandler : IOperationResultCommandHandler<CommandUpdateRole>
{
    private static readonly Dictionary<Type, StudyRole> StudyRoles = new()
    {
        { typeof(Student), StudyRole.Student },
        { typeof(Teacher), StudyRole.Teacher }
    };
        
    private const string ErrorCode = "UpdateProfile.ProfileNotFound";
    
    private readonly ApplicationDbContext _dbContext;

    public CommandUpdateRoleHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<OperationResult> Handle(CommandUpdateRole request, CancellationToken cancellationToken)
    {
        var studentUpdate = await UpdateRole<Student, Teacher>(request, cancellationToken);

        if (studentUpdate.IsSuccess)
        {
            return studentUpdate;
        }

        return await UpdateRole<Teacher, Student>(request, cancellationToken);
    }
    
    private async Task<OperationResult<bool>> UpdateRole<TFrom, TTo>(
        CommandUpdateRole request, CancellationToken cancellationToken) 
        where TFrom : Person, new()
        where TTo : Person, new()
    {
        var repository = _dbContext.Set<TFrom>();
        var profile = await repository.FirstOrDefaultAsync(
            x => x.UserName.ToLower() == request.UserName, cancellationToken);

        if (profile is null)
        {
            return OperationResult.Failed<bool>(ErrorCode);
        }

        profile.Role = (ApplicationRole)request.AppRole;

        if (request.StudyRole == StudyRoles[typeof(TFrom)])
        {
            repository.Update(profile);
        }
        else
        {
            _dbContext.Remove(profile);
            await _dbContext.Set<TTo>().AddAsync(profile.Map<TTo>(), cancellationToken);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return OperationResult.Successful(true);
    }
}