using PhlegmaticOne.JwtTokensGeneration.Models;

namespace PhlegmaticOne.JwtTokensGeneration;

public interface IJwtTokenGenerator
{
    JwtOptionsResult GenerateToken(UserRegisteringModel userRegisteringModel);
}