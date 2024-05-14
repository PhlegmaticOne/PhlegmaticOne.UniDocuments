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
using StudyRole = UniDocuments.App.Shared.Users.Enums.StudyRole;

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
        var login = request.LoginObject;
        
        try
        {
            var profile = login.StudyRole switch
            {
                StudyRole.Student => await GetProfileAsync<Student>(request, cancellationToken),
                StudyRole.Teacher => await GetProfileAsync<Teacher>(request, cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(login.StudyRole))
            };

            return profile is null ? 
                OperationResult.Failed<ProfileObject>(AuthorizeProfileNotExist) :
                OperationResult.Successful(profile);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, ErrorMessage);
            return OperationResult.Failed<ProfileObject>(ErrorMessage, e.Message);
        }
    }

    private async Task<ProfileObject?> GetProfileAsync<T>(
        QueryLogin request, CancellationToken cancellationToken) where T : Person, new()
    {
        var login = request.LoginObject;
        var repository = _dbContext.Set<T>();
        var password = _passwordHasher.Hash(login.Password);

        var person = await repository.FirstOrDefaultAsync(
            predicate: p => p.UserName == login.UserName && p.Password == password,
            cancellationToken: cancellationToken);

        return person is null ? null : _profileSetuper.SetupFrom(person);
    }
}