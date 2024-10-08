﻿using PhlegmaticOne.JwtTokensGeneration;
using PhlegmaticOne.JwtTokensGeneration.Models;
using UniDocuments.App.Domain.Services.Common;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Services.Common;

public class JwtTokenGenerationService : IJwtTokenGenerationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public JwtTokenGenerationService(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public JwtTokenObject GenerateJwtToken(ProfileObject profile)
    {
        var userInfo = new UserRegisteringModel(
            profile.Id, (int)profile.StudyRole, (int)profile.AppRole, profile.JoinDate,
            profile.FirstName, profile.LastName, profile.UserName);

        var tokenResult = _jwtTokenGenerator.GenerateToken(userInfo);
        
        return new JwtTokenObject
        {
            Token = tokenResult.Token
        };
    }
}