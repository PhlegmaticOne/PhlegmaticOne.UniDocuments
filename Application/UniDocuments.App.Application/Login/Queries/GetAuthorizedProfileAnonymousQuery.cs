using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PasswordHasher;
using UniDocuments.App.Data.EntityFramework.Context;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Application.Login.Queries;

public class GetAuthorizedProfileAnonymousQuery : IOperationResultQuery<AuthorizedProfileDto>
{
    public GetAuthorizedProfileAnonymousQuery(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public string UserName { get; }
    public string Password { get; }
}

public class GetAuthorizedProfileAnonymousQueryHandler :
    IOperationResultQueryHandler<GetAuthorizedProfileAnonymousQuery, AuthorizedProfileDto>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IJwtTokenGenerationService _jwtTokenGenerationService;
    private readonly IPasswordHasher _passwordHasher;

    public GetAuthorizedProfileAnonymousQueryHandler(
        ApplicationDbContext dbContext,
        IJwtTokenGenerationService jwtTokenGenerationService,
        IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _jwtTokenGenerationService = jwtTokenGenerationService;
        _passwordHasher = passwordHasher;
    }


    public async Task<OperationResult<AuthorizedProfileDto>> Handle(
        GetAuthorizedProfileAnonymousQuery request, CancellationToken cancellationToken)
    {
        var authorizedProfile = await GetAuthorizedProfileAsync(request, cancellationToken);

        if (authorizedProfile is null)
        {
            return OperationResult.Failed<AuthorizedProfileDto>("Profile does not exist");
        }

        authorizedProfile.JwtToken = _jwtTokenGenerationService.GenerateJwtToken(authorizedProfile);

        return OperationResult.Successful(authorizedProfile);
    }

    private async Task<AuthorizedProfileDto?> GetAuthorizedProfileAsync(GetAuthorizedProfileAnonymousQuery request,
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

        return new AuthorizedProfileDto
        {
            UserName = student.UserName,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Id = student.Id
        };
    }
}