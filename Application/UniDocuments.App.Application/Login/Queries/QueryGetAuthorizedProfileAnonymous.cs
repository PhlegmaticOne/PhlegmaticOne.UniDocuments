using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PasswordHasher;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Application.Login.Queries;

public class QueryGetAuthorizedProfileAnonymous : IOperationResultQuery<AuthorizedProfileObject>
{
    public QueryGetAuthorizedProfileAnonymous(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public string UserName { get; }
    public string Password { get; }
}

public class QueryGetAuthorizedProfileAnonymousHandler :
    IOperationResultQueryHandler<QueryGetAuthorizedProfileAnonymous, AuthorizedProfileObject>
{
    private const string AuthorizeProfileNotExist = "AuthorizeProfile.NotExist";
    private const string ErrorMessage = "GetAuthorizedProfile.InternalError";
    
    private readonly ApplicationDbContext _dbContext;
    private readonly IJwtTokenGenerationService _jwtTokenGenerationService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<QueryGetAuthorizedProfileAnonymousHandler> _logger;

    public QueryGetAuthorizedProfileAnonymousHandler(
        ApplicationDbContext dbContext,
        IJwtTokenGenerationService jwtTokenGenerationService,
        IPasswordHasher passwordHasher,
        ILogger<QueryGetAuthorizedProfileAnonymousHandler> logger)
    {
        _dbContext = dbContext;
        _jwtTokenGenerationService = jwtTokenGenerationService;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }


    public async Task<OperationResult<AuthorizedProfileObject>> Handle(
        QueryGetAuthorizedProfileAnonymous request, CancellationToken cancellationToken)
    {
        try
        {
            var authorizedProfile = await GetAuthorizedProfileAsync(request, cancellationToken);

            if (authorizedProfile is null)
            {
                return OperationResult.Failed<AuthorizedProfileObject>(AuthorizeProfileNotExist);
            }

            authorizedProfile.JwtToken = _jwtTokenGenerationService.GenerateJwtToken(authorizedProfile);

            return OperationResult.Successful(authorizedProfile);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, ErrorMessage);
            return OperationResult.Failed<AuthorizedProfileObject>(ErrorMessage, e.Message);
        }
    }

    private async Task<AuthorizedProfileObject?> GetAuthorizedProfileAsync(QueryGetAuthorizedProfileAnonymous request,
        CancellationToken cancellationToken)
    {
        var repository = _dbContext.Set<Student>();
        var password = _passwordHasher.Hash(request.Password);

        var student = await repository.FirstOrDefaultAsync(
            predicate: p => p.UserName == request.UserName && p.Password == password,
            cancellationToken: cancellationToken);

        if (student is null)
        {
            return null;
        }

        return new AuthorizedProfileObject
        {
            UserName = student.UserName,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Id = student.Id
        };
    }
}