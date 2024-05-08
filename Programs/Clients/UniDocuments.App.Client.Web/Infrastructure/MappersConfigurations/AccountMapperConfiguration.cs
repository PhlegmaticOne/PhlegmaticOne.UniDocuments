using AutoMapper;
using UniDocuments.App.Client.Web.ViewModels.Account;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Infrastructure.MappersConfigurations;

public class AccountMapperConfiguration : Profile
{
    public AccountMapperConfiguration()
    {
        CreateMap<RegisterViewModel, RegisterObject>();
        CreateMap<LoginViewModel, LoginObject>();
    }
}