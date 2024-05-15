using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using PhlegmaticOne.OperationResults;
using UniDocuments.App.Application.Infrastructure.Extensions;
using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Api.Infrastructure.Roles;

public class RequireAppRolesMiddleware
{
    private readonly RequestDelegate _next;

    public RequireAppRolesMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated)
        {
            await _next(context);
            return;
        }
        
        var appRole = context.User.AppRole();
        var endpoint = context.GetEndpoint();

        if (endpoint is not null && appRole != AppRole.Admin)
        {
            var endpointActionData = endpoint.Metadata.GetRequiredMetadata<ControllerActionDescriptor>();

            var appRolesAttribute = endpointActionData.MethodInfo.GetCustomAttribute<RequireAppRolesAttribute>();

            if (appRolesAttribute is not null && appRolesAttribute.AppRoles.Contains(appRole) == false)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(GetResponse());
                return;
            }
        }
        
        await _next(context);
    }
    
    private static string GetResponse()
    {
        return JsonConvert.SerializeObject(OperationResult.Failed("Restricted by app role"));
    }
}