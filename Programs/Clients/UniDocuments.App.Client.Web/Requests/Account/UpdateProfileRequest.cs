using PhlegmaticOne.ApiRequesting.Models.Requests;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Requests.Account;

public class UpdateProfileRequest : ClientPutRequest<UpdateProfileObject, ProfileObject>
{
    public UpdateProfileRequest(UpdateProfileObject requestData) : base(requestData)
    {
    }
}