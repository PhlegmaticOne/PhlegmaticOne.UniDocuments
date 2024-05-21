using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PasswordHasher;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Models.Base;
using UniDocuments.App.Domain.Services.Profiles;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Application.App.Login.Queries;

public class QueryLogin : IOperationResultQuery<ProfileObject>
{
    public LoginObject LoginObject { get; }

    public QueryLogin(LoginObject loginObject)
    {
        LoginObject = loginObject;
    }
}

public class QueryLoginHandler : IOperationResultQueryHandler<QueryLogin, ProfileObject>
{
    private const string AuthorizeProfileNotExist = "AuthorizeProfile.NotExist";
    private const string ErrorMessage = "GetAuthorizedProfile.InternalError";
    
    private readonly ApplicationDbContext _dbContext;
    private readonly IProfileSetuper _profileSetuper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<QueryLoginHandler> _logger;

    public QueryLoginHandler(
        ApplicationDbContext dbContext,
        IProfileSetuper profileSetuper,
        IPasswordHasher passwordHasher,
        ILogger<QueryLoginHandler> logger)
    {
        _dbContext = dbContext;
        _profileSetuper = profileSetuper;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<OperationResult<ProfileObject>> Handle(QueryLogin request, CancellationToken cancellationToken)
    {
        try
        {
            var password = _passwordHasher.Hash(request.LoginObject.Password);
            var student = await GetProfileAsync<Student>(request, password, cancellationToken);

            if (student is not null)
            {
                return OperationResult.Successful(student);
            }

            var teacher = await GetProfileAsync<Teacher>(request, password, cancellationToken);

            return teacher is null ? 
                OperationResult.Failed<ProfileObject>(AuthorizeProfileNotExist) :
                OperationResult.Successful(teacher);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, ErrorMessage);
            return OperationResult.Failed<ProfileObject>(ErrorMessage, e.Message);
        }
    }

    private async Task<ProfileObject?> GetProfileAsync<T>(
        QueryLogin request, string password, CancellationToken cancellationToken) where T : Person, new()
    {
        var login = request.LoginObject;
        var repository = _dbContext.Set<T>();
        var person = await repository.FirstOrDefaultAsync(
            p => p.UserName.ToLower() == login.UserName, cancellationToken);

        if (person is null || password != person.Password)
        {
            return null;
        }

        return _profileSetuper.SetupFrom(person);
    }
}