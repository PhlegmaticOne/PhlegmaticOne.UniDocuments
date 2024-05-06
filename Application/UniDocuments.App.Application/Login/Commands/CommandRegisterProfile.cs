using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PasswordHasher;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Application.Login.Commands;

public class CommandRegisterProfile : IOperationResultCommand
{
    public CommandRegisterProfile(RegisterProfileObject registerProfileObject)
    {
        RegisterProfileObject = registerProfileObject;
    }

    public RegisterProfileObject RegisterProfileObject { get; }
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
            var prepared = PrepareProfile(request.RegisterProfileObject);
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
    
    private Student PrepareProfile(RegisterProfileObject registerProfileObject)
    {
        return new Student
        {
            FirstName = registerProfileObject.FirstName,
            LastName = registerProfileObject.LastName,
            UserName = registerProfileObject.UserName,
            Password = _passwordHasher.Hash(registerProfileObject.Password)
        };
    }
}