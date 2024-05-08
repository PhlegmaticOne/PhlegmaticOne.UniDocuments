using System.Security.Claims;
using AutoMapper;
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
    protected readonly IMapper Mapper;
    protected readonly IClientRequestsService ClientRequestsService;
    protected readonly ILocalStorageService LocalStorageService;

    public ClientRequestsController(
        IClientRequestsService clientRequestsService, 
        ILocalStorageService localStorageService,
        IMapper mapper)
    {
        Mapper = mapper;
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
        return Redirect("/Auth/Login");
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

    protected Task AuthenticateAsync(ProfileObject profileObject)
    {
        var claimsPrincipal = ClaimsPrincipalGenerator.GenerateClaimsPrincipal(profileObject);
        return SignInAsync(profileObject.Id, claimsPrincipal, profileObject.JwtToken);
    }

    protected Task SignOutAsync()
    {
        return HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    protected Task SignInAsync(Guid id, ClaimsPrincipal claimsPrincipal, JwtTokenObject jwtToken)
    {
        SetJwtToken(id, jwtToken);
        return HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
    }

    protected string? JwtToken()
    {
        var id = User.IdString();

        if (id is null)
        {
            return null;
        }
        
        var token = LocalStorageService.GetValue<JwtTokenObject>(id);
        return token?.Token;
    }

    protected void SetJwtToken(Guid id, JwtTokenObject jwtToken)
    {
        LocalStorageService.SetValue(id.ToString(), jwtToken, TimeSpan.FromMinutes(jwtToken.ExpirationInMinutes));
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