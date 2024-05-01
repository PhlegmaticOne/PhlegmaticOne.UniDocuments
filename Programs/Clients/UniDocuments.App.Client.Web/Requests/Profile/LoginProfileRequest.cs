﻿using PhlegmaticOne.ApiRequesting.Models.Requests;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Requests.Profile;

public class LoginProfileRequest : ClientPostRequest<LoginObject, AuthorizedProfileObject>
{
    public LoginProfileRequest(LoginObject requestData) : base(requestData)
    {
    }
}