using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PasswordHasher;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Shared.Users;
using StudyRole = UniDocuments.App.Shared.Users.Enums.StudyRole;

namespace UniDocuments.App.Application.App.Profile;

public class CommandUpdateProfile : IdentityOperationResultCommand
{
    public UpdateProfileObject UpdateProfileObject { get; }
    
    public CommandUpdateProfile(Guid profileId, UpdateProfileObject updateProfileObject) : base(profileId)
    {
        UpdateProfileObject = updateProfileObject;
    }
}

public class CommandUpdateProfileHandler : IOperationResultCommandHandler<CommandUpdateProfile>
{
    private const string ErrorCode = "UpdateProfile.ProfileNotFound";
    
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerationService _jwtTokenGenerationService;

    public CommandUpdateProfileHandler(
        ApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerationService jwtTokenGenerationService)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerationService = jwtTokenGenerationService;
    }

    public async Task<OperationResult> Handle(CommandUpdateProfile request, CancellationToken cancellationToken)
    {
        var updateProfileObject = request.UpdateProfileObject;
        var repository = _dbContext.Set<Student>();
        var profile = await repository.FirstOrDefaultAsync(x => x.Id == request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return OperationResult.Failed<ProfileObject>(ErrorCode);
        }

        var oldPassword = _passwordHasher.Hash(updateProfileObject.OldPassword);

        if (profile.Password != oldPassword)
        {
            return OperationResult.Failed<ProfileObject>("Wrong password!");
        }

        profile.FirstName = GetNewValueOrExisting(updateProfileObject.FirstName, profile.FirstName);
        profile.LastName = GetNewValueOrExisting(updateProfileObject.LastName, profile.LastName);
        profile.UserName = GetNewValueOrExisting(updateProfileObject.UserName, profile.UserName);
        profile.Password = ProcessPassword(profile.Password, updateProfileObject.NewPassword);
        
        repository.Update(profile);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var result = new ProfileObject
        {
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            UserName = profile.UserName,
            Id = request.ProfileId,
            Role = (StudyRole)profile.Role,
        };

        result.JwtToken = _jwtTokenGenerationService.GenerateJwtToken(result);
        
        return OperationResult.Successful(result);
    }
    
    private string ProcessPassword(string oldPassword, string newPassword)
    {
        return string.IsNullOrEmpty(newPassword) ? oldPassword : _passwordHasher.Hash(newPassword);
    }
    
    private static string GetNewValueOrExisting(string newValue, string existing)
    {
        return string.IsNullOrEmpty(newValue) == false ? newValue : existing;
    }
}