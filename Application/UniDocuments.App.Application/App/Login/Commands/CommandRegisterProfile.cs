using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PasswordHasher;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Users;

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
    private readonly ILogger<CommandRegisterProfileHandler> _logger;

    public CommandRegisterProfileHandler(
        ApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        ILogger<CommandRegisterProfileHandler> logger)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }
    
    public async Task<OperationResult> Handle(CommandRegisterProfile request, CancellationToken cancellationToken)
    {
        try
        {
            var prepared = PrepareProfile(request.RegisterObject);
            var repository = _dbContext.Set<Student>();
            var entry = await repository.AddAsync(prepared, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return OperationResult.Successful(entry.Entity.Id);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, RegisterProfileInternalError);
            return OperationResult.Failed(RegisterProfileInternalError, e.Message);
        }
    }
    
    private Student PrepareProfile(RegisterObject registerObject)
    {
        return new Student
        {
            FirstName = registerObject.FirstName,
            LastName = registerObject.LastName,
            UserName = registerObject.UserName,
            Password = _passwordHasher.Hash(registerObject.Password),
            Role = StudyRole.Default
        };
    }
}