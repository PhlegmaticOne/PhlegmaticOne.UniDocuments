using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Domain.Services.Common;

public interface IJwtTokenGenerationService
{
    JwtTokenObject GenerateJwtToken(ProfileObject profile);
}