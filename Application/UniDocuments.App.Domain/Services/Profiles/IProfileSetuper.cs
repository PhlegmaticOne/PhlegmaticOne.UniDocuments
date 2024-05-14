using UniDocuments.App.Domain.Models.Base;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Domain.Services.Profiles;

public interface IProfileSetuper
{
    ProfileObject SetupFrom<T>(T person) where T : Person;
}