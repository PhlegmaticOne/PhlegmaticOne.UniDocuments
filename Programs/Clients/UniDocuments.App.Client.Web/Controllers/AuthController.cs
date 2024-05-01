using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.ApiRequesting.Services;
using PhlegmaticOne.LocalStorage;
using UniDocuments.App.Client.Web.Controllers.Base;
using UniDocuments.App.Client.Web.Requests.Profile;
using UniDocuments.App.Client.Web.ViewModels.Account;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Controllers;

[AllowAnonymous]
public class AuthController : ClientRequestsController
{
    public AuthController(
        IClientRequestsService clientRequestsService, 
        ILocalStorageService localStorageService) :
        base(clientRequestsService, localStorageService)
    {
    }

    [HttpGet]
    public IActionResult Login(string returnUrl) => View();

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        var dto = new RegisterProfileObject
        {
            Password = registerViewModel.Password,
            FirstName = registerViewModel.FirstName,
            LastName = registerViewModel.LastName,
            UserName = registerViewModel.Email
        };
        
        return await FromAuthorizedPost(new RegisterProfileRequest(dto), async profile =>
        {
            await AuthenticateAsync(profile);
            return RedirectToAction("Index", "Home");
        }, result => ViewWithErrorsFromOperationResult(result, nameof(Register), registerViewModel));
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        var dto = new LoginObject
        {
            Password = loginViewModel.Password,
            UserName = loginViewModel.Email
        };
        
        return await FromAuthorizedPost(new LoginProfileRequest(dto), async profile =>
        {
            await AuthenticateAsync(profile);
            return loginViewModel.ReturnUrl is null
                ? RedirectToAction("Index", "Home")
                : LocalRedirect(loginViewModel.ReturnUrl);
        }, result => ViewWithErrorsFromOperationResult(result, nameof(Login), loginViewModel));
    }
}