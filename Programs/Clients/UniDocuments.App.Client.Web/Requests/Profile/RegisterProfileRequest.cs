using PhlegmaticOne.ApiRequesting.Models.Requests;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Requests.Profile;

public class RegisterProfileRequest : ClientPostRequest<RegisterProfileObject, AuthorizedProfileObject>
{
    public RegisterProfileRequest(RegisterProfileObject requestData) : base(requestData)
    {
    }
}