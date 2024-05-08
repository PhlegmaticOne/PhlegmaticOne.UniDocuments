using System.Security.Claims;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.ApiRequesting.Models;
using PhlegmaticOne.ApiRequesting.Models.Requests;
using PhlegmaticOne.ApiRequesting.Services;
using PhlegmaticOne.LocalStorage;
using PhlegmaticOne.OperationResults;
using UniDocuments.App.Client.Web.Infrastructure.Extensions;
using UniDocuments.App.Client.Web.Infrastructure.Helpers;
using UniDocuments.App.Client.Web.ViewModels.Base;
using UniDocuments.App.Shared.Users;

namespace UniDocuments.App.Client.Web.Controllers.Base;

public class ClientRequestsController : Controller
{
    protected readonly IClientRequestsService ClientRequestsService;
    protected readonly ILocalStorageService LocalStorageService;

    public ClientRequestsController(IClientRequestsService clientRequestsService, ILocalStorageService localStorageService)
    {
        LocalStorageService = localStorageService;
        ClientRequestsService = clientRequestsService;
    }

    protected async Task<IActionResult> FromAuthorizedGet<TRequest, TResponse>(
        ClientGetRequest<TRequest, TResponse> clientGetRequest,
        Func<TResponse, Task<IActionResult>> onSuccess,
        Func<OperationResult, IActionResult>? onOperationFailed = null,
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null)
    {
        var serverResponse = await ClientRequestsService.GetAsync(clientGetRequest, JwtToken());
        return await HandleResponse(serverResponse, onSuccess, onOperationFailed, onServerResponseFailed);
    }

    protected async Task<IActionResult> FromAuthorizedPost<TRequest, TResponse>(
        ClientPostRequest<TRequest, TResponse> clientPostRequest,
        Func<TResponse, Task<IActionResult>> onSuccess,
        Func<OperationResult, IActionResult>? onOperationFailed = null,
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null)
    {
        var serverResponse = await ClientRequestsService.PostAsync(clientPostRequest, JwtToken());
        return await HandleResponse(serverResponse, onSuccess, onOperationFailed, onServerResponseFailed);
    }

    protected async Task<IActionResult> FromAuthorizedPut<TRequest, TResponse>(
        ClientPutRequest<TRequest, TResponse> clientPostRequest,
        Func<TResponse, Task<IActionResult>> onSuccess,
        Func<OperationResult, IActionResult>? onOperationFailed = null,
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null)
    {
        var serverResponse = await ClientRequestsService.PutAsync(clientPostRequest, JwtToken());
        return await HandleResponse(serverResponse, onSuccess, onOperationFailed, onServerResponseFailed);
    }

    protected IActionResult LoginView()
    {
        return Redirect("/Account/Login");
    }

    protected IActionResult ErrorView(string errorMessage)
    {
        return RedirectToAction("Error", "Home", new { errorMessage });
    }

    protected IActionResult HomeView()
    {
        return Redirect("/");
    }

    protected IActionResult ViewWithErrorsFromOperationResult(
        OperationResult operationResult, string viewName, ErrorHavingViewModel viewModel)
    {
        viewModel.ErrorMessage = operationResult.ErrorMessage;
        return View(viewName, viewModel);
    }

    protected IActionResult ViewWithErrorsFromValidationResult(
        ValidationResult validationResult, string viewName, ErrorHavingViewModel viewModel)
    {
        validationResult.AddToModelState(ModelState);
        return View(viewName, viewModel);
    }

    protected async Task AuthenticateAsync(ProfileObject profileObject)
    {
        var claimsPrincipal = ClaimsPrincipalGenerator.GenerateClaimsPrincipal(profileObject);
        await SignInAsync(claimsPrincipal, profileObject.JwtToken);
    }

    protected async Task SignOutAsync()
    {
        SetJwtToken(new JwtTokenObject(string.Empty));
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    protected async Task SignInAsync(ClaimsPrincipal claimsPrincipal, JwtTokenObject jwtToken)
    {
        SetJwtToken(jwtToken);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
    }

    protected string? JwtToken()
    {
        return LocalStorageService.GetValue<JwtTokenObject>(User.Username())!.Token;
    }

    protected void SetJwtToken(JwtTokenObject jwtToken)
    {
        LocalStorageService.SetValue(User.Username(), jwtToken);
    }

    private async Task<IActionResult> HandleResponse<TResponse>(
        ServerResponse<TResponse> serverResponse,
        Func<TResponse, Task<IActionResult>> onSuccess,
        Func<OperationResult, IActionResult>? onOperationFailed = null,
        Func<ServerResponse, IActionResult>? onServerResponseFailed = null)
    {
        if (serverResponse.IsUnauthorized)
        {
            await SignOutAsync();
            return LoginView();
        }

        if (serverResponse.IsSuccess == false)
        {
            return onServerResponseFailed is not null
                ? onServerResponseFailed(serverResponse)
                : ErrorView(serverResponse.ToString());
        }

        var operationResult = serverResponse.OperationResult!;

        if (operationResult.IsSuccess == false)
        {
            return onOperationFailed is not null
                ? onOperationFailed(operationResult)
                : ErrorView(operationResult.ErrorMessage!);
        }

        var data = serverResponse.GetData()!;
        return await onSuccess(data);
    }
}