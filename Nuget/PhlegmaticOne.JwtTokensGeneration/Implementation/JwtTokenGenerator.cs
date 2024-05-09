using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.JwtTokensGeneration.Helpers;
using PhlegmaticOne.JwtTokensGeneration.Models;
using PhlegmaticOne.JwtTokensGeneration.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PhlegmaticOne.JwtTokensGeneration.Implementation;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IJwtOptions _jwtOptions;

    public JwtTokenGenerator(IJwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public JwtOptionsResult GenerateToken(UserRegisteringModel userRegisteringModel)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userRegisteringModel.UserName),
            new(JwtRegisteredClaimNames.GivenName, userRegisteringModel.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, userRegisteringModel.LastName),
            new(CustomJwtClaimNames.UserId, userRegisteringModel.Id.ToString()),
            new(CustomJwtClaimNames.AppRole, userRegisteringModel.AppRole.ToString()),
            new(CustomJwtClaimNames.StudyRole, userRegisteringModel.StudyRole.ToString()),
        };

        var securityKey = _jwtOptions.GetSecretKey();
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationDurationInMinutes),
            signingCredentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return new JwtOptionsResult(tokenString, _jwtOptions.ExpirationDurationInMinutes);
    }
}