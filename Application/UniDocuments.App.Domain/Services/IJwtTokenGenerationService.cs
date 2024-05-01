using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Domain.Services;

public interface IJwtTokenGenerationService
{
    JwtTokenObject GenerateJwtToken(AuthorizedProfileObject profile);
}