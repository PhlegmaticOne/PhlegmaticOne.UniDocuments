using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PasswordHasher;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Application.Login.Commands;

public class RegisterProfileCommand : IOperationResultCommand
{
    public RegisterProfileCommand(RegisterProfileDto registerProfileModel)
    {
        RegisterProfileModel = registerProfileModel;
    }

    public RegisterProfileDto RegisterProfileModel { get; }
}

public class RegisterProfileCommandHandler : IOperationResultCommandHandler<RegisterProfileCommand>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterProfileCommandHandler(ApplicationDbContext dbContext, IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<OperationResult> Handle(RegisterProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var prepared = PrepareProfile(request.RegisterProfileModel);
            var repository = _dbContext.Set<Student>();
            await repository.AddAsync(prepared, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return OperationResult.Success;
        }
        catch (Exception e)
        {
            return OperationResult.Failed("RegisterProfile.InternalError", e.Message);
        }
    }
    
    private Student PrepareProfile(RegisterProfileDto registerProfileDto)
    {
        return new Student
        {
            FirstName = registerProfileDto.FirstName,
            LastName = registerProfileDto.LastName,
            UserName = registerProfileDto.UserName,
            Password = _passwordHasher.Hash(registerProfileDto.Password)
        };
    }
}