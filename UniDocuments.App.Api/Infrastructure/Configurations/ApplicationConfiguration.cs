﻿using PhlegmaticOne.JwtTokensGeneration.Options;

namespace UniDocuments.App.Api.Infrastructure.Configurations;

public class ApplicationConfiguration
{
    public bool UseRealDatabase { get; set; }
    public string CurrentKerasOptions { get; set; } = null!;
    public ApplicationJwtSecrets JwtSecrets { get; set; } = null!;
    public string[] IncludePythonScripts { get; set; } = null!;
    public string SavePath { get; set; } = null!;
    public DateTime StartTime { get; set; }

    public IJwtOptions CreateJwtOptions()
    {
        return new SymmetricKeyJwtOptions(
            JwtSecrets.Issuer,
            JwtSecrets.Audience,
            JwtSecrets.ExpirationDurationInMinutes,
            JwtSecrets.SecretKey);
    }
}