using PhlegmaticOne.ApiRequesting.Models.Requests;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Requests.Profile;

public class LoginProfileRequest : ClientPostRequest<LoginDto, AuthorizedProfileDto>
{
    public LoginProfileRequest(LoginDto requestData) : base(requestData)
    {
    }
}