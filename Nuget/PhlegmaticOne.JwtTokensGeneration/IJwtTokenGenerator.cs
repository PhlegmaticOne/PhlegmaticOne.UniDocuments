using PhlegmaticOne.JwtTokensGeneration.Models;

namespace PhlegmaticOne.JwtTokensGeneration;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserRegisteringModel userRegisteringModel);
}