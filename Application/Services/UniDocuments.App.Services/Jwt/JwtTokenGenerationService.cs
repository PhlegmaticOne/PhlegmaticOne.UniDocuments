using PhlegmaticOne.JwtTokensGeneration;
using PhlegmaticOne.JwtTokensGeneration.Models;
using UniDocuments.App.Domain.Services;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Services.Jwt;

public class JwtTokenGenerationService : IJwtTokenGenerationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public JwtTokenGenerationService(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public JwtTokenDto GenerateJwtToken(AuthorizedProfileDto profile)
    {
        var userInfo = new UserRegisteringModel(profile.Id, profile.FirstName, profile.LastName, profile.UserName);
        var tokenValue = _jwtTokenGenerator.GenerateToken(userInfo);
        return new JwtTokenDto(tokenValue);
    }
}