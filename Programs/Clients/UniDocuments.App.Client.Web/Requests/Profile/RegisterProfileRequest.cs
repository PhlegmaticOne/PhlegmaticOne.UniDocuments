using PhlegmaticOne.ApiRequesting.Models.Requests;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Requests.Profile;

public class RegisterProfileRequest : ClientPostRequest<RegisterProfileDto, AuthorizedProfileDto>
{
    public RegisterProfileRequest(RegisterProfileDto requestData) : base(requestData)
    {
    }
}