using AutoMapper;
using UniDocuments.App.Domain.Models;
using UniDocuments.App.Shared.Activities.Detailed;

namespace UniDocuments.App.Application.Infrastructure.Mappers;

public class ActivitiesMapper : Profile
{
    public ActivitiesMapper()
    {
        CreateMap<StudyActivity, ActivityDetailedObject>()
            .ForMember(x => x.CreatorFirstName, o => o.MapFrom(x => x.Creator.FirstName))
            .ForMember(x => x.CreatorLastName, o => o.MapFrom(x => x.Creator.LastName));
    }
}