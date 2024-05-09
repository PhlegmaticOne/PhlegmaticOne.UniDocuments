using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PasswordHasher;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Models.Base;
using UniDocuments.App.Domain.Models.Enums;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Shared.Users;
using StudyRole = UniDocuments.App.Shared.Users.Enums.StudyRole;

namespace UniDocuments.App.Application.App.Profile;

public class CommandUpdateProfile : IdentityOperationResultCommand
{
    public StudyRole StudyRole { get; }
    public UpdateProfileObject UpdateProfileObject { get; }
    
    public CommandUpdateProfile(Guid profileId, StudyRole studyRole, UpdateProfileObject updateProfileObject) : base(profileId)
    {
        StudyRole = studyRole;
        UpdateProfileObject = updateProfileObject;
    }
}

public class CommandUpdateProfileHandler : IOperationResultCommandHandler<CommandUpdateProfile>
{
    private const string ErrorCode = "UpdateProfile.ProfileNotFound";
    private const string WrongPassword = "Wrong password!";
    
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IProfileSetuper _profileSetuper;

    public CommandUpdateProfileHandler(
        ApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        IProfileSetuper profileSetuper)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _profileSetuper = profileSetuper;
    }

    public async Task<OperationResult> Handle(CommandUpdateProfile request, CancellationToken cancellationToken)
    {
        return request.StudyRole switch
        {
            StudyRole.Student => await UpdateProfile<Student>(request, cancellationToken),
            StudyRole.Teacher => await UpdateProfile<Teacher>(request, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(request.StudyRole))
        };
    }

    private async Task<OperationResult<ProfileObject>> UpdateProfile<T>(
        CommandUpdateProfile request, CancellationToken cancellationToken) where T : Person, new()
    {
        var updateProfileObject = request.UpdateProfileObject;
        
        var repository = _dbContext.Set<T>();
        var profile = await repository.FirstOrDefaultAsync(x => x.Id == request.ProfileId, cancellationToken);

        if (profile is null)
        {
            return OperationResult.Failed<ProfileObject>(ErrorCode);
        }

        var oldPassword = _passwordHasher.Hash(updateProfileObject.OldPassword);

        if (profile.Password != oldPassword)
        {
            return OperationResult.Failed<ProfileObject>(WrongPassword);
        }

        UpdateProfileProperties(profile, updateProfileObject);
        repository.Update(profile);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var result = _profileSetuper.SetupFrom(profile);
        return OperationResult.Successful(result);
    }

    private void UpdateProfileProperties(Person profile, UpdateProfileObject updateProfileObject)
    {
        profile.FirstName = GetNewValueOrExisting(updateProfileObject.FirstName, profile.FirstName);
        profile.LastName = GetNewValueOrExisting(updateProfileObject.LastName, profile.LastName);
        profile.UserName = GetNewValueOrExisting(updateProfileObject.UserName, profile.UserName);
        profile.Password = ProcessPassword(profile.Password, updateProfileObject.NewPassword);
        profile.Role = (ApplicationRole)updateProfileObject.AppRole;
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