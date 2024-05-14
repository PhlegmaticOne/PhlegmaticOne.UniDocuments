using AutoMapper;
using UniDocuments.App.Domain.Models.Base;
using UniDocuments.App.Domain.Services.Common;
using UniDocuments.App.Domain.Services.Profiles;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Services.Profiles;

public class ProfileSetuper : IProfileSetuper
{
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenerationService _jwtTokenGenerationService;

    public ProfileSetuper(IMapper mapper, IJwtTokenGenerationService jwtTokenGenerationService)
    {
        _mapper = mapper;
        _jwtTokenGenerationService = jwtTokenGenerationService;
    }
    
    public ProfileObject SetupFrom<T>(T person) where T : Person
    {
        var profile = _mapper.Map<ProfileObject>(person);
        profile.JwtToken = _jwtTokenGenerationService.GenerateJwtToken(profile);
        return profile;
    }
}