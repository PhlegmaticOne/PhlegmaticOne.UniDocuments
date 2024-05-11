using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PasswordHasher;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Models.Base;
using UniDocuments.App.Domain.Models.Enums;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Shared.Users;
using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Application.App.Login.Commands;

public class CommandRegisterProfile : IOperationResultCommand
{
    public CommandRegisterProfile(RegisterObject registerObject)
    {
        RegisterObject = registerObject;
    }

    public RegisterObject RegisterObject { get; }
}

public class CommandRegisterProfileHandler : IOperationResultCommandHandler<CommandRegisterProfile>
{
    private const string RegisterProfileInternalError = "RegisterProfile.InternalError";
    
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IProfileSetuper _profileSetuper;
    private readonly ITimeProvider _timeProvider;
    private readonly ILogger<CommandRegisterProfileHandler> _logger;

    public CommandRegisterProfileHandler(
        ApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        IProfileSetuper profileSetuper,
        ITimeProvider timeProvider,
        ILogger<CommandRegisterProfileHandler> logger)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _profileSetuper = profileSetuper;
        _timeProvider = timeProvider;
        _logger = logger;
    }
    
    public async Task<OperationResult> Handle(CommandRegisterProfile request, CancellationToken cancellationToken)
    {
        var register = request.RegisterObject;
        
        try
        {
            var profile = register.StudyRole switch
            {
                StudyRole.Student => await CreateProfileAsync<Student>(register, cancellationToken),
                StudyRole.Teacher => await CreateProfileAsync<Teacher>(register, cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(register.StudyRole))
            };
            
            return OperationResult.Successful(profile);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, RegisterProfileInternalError);
            return OperationResult.Failed(RegisterProfileInternalError, e.Message);
        }
    }

    private async Task<ProfileObject> CreateProfileAsync<T>(
        RegisterObject registerObject, CancellationToken cancellationToken) where T : Person, new()
    {
        var profile = PrepareProfile<T>(registerObject);
        var repository = _dbContext.Set<T>();
        var entry = await repository.AddAsync(profile, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return _profileSetuper.SetupFrom(entry.Entity);
    }
    
    private T PrepareProfile<T>(RegisterObject registerObject) where T : Person, new()
    {
        return new T
        {
            FirstName = registerObject.FirstName,
            LastName = registerObject.LastName,
            UserName = registerObject.UserName,
            Password = _passwordHasher.Hash(registerObject.Password),
            Role = ApplicationRole.Default,
            JoinDate = _timeProvider.Now
        };
    }
}