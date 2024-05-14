using AutoMapper;
using UniDocuments.App.Api.Controllers.Documents;
using UniDocuments.App.Application.Documents.Loading.Commands;
using UniDocuments.App.Application.Documents.Reports;
using UniDocuments.App.Shared.Documents;

namespace UniDocuments.App.Api.Infrastructure.Mappers;

public class MapperConfiguration : Profile
{
    public MapperConfiguration()
    {
        CreateMap<DocumentBuildReportRequest, QueryBuildPlagiarismDocumentReport>()
            .ForMember(x => x.FileName, o => o.MapFrom(x => x.File.FileName))
            .ForMember(x => x.FileStream, o => o.MapFrom(x => x.File.OpenReadStream()));

        CreateMap<DocumentUploadObject, CommandUploadDocument>()
            .ForMember(x => x.FileName, o => o.MapFrom(x => x.File.FileName))
            .ForMember(x => x.DocumentStream, o => o.MapFrom(x => x.File.OpenReadStream()))
            .ForMember(x => x.ActivityId, o => o.MapFrom(x => x.Id))
            .ForMember(x => x.ProfileId, o => o.Ignore());
    }
}