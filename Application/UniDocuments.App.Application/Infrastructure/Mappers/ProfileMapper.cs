using AutoMapper;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Domain.Models.Base;
using UniDocuments.App.Shared.Users;
using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Application.Infrastructure.Mappers;

public class ProfileMapper : Profile
{
    public ProfileMapper()
    {
        CreateMap<Person, ProfileObject>()
            .ForMember(x => x.StudyRole, o => o.MapFrom(x => StudyRole.Student))
            .ForMember(x => x.AppRole, o => o.MapFrom(x => (AppRole)x.Role));
        
        CreateMap<Teacher, ProfileObject>()
            .ForMember(x => x.StudyRole, o => o.MapFrom(x => StudyRole.Teacher))
            .ForMember(x => x.AppRole, o => o.MapFrom(x => (AppRole)x.Role));
    }
}