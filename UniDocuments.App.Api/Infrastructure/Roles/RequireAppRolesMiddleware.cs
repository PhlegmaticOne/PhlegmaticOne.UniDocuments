using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
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
        var appRole = context.User.AppRole();
        var endpoint = context.GetEndpoint();

        if (endpoint is not null && appRole != AppRole.Admin)
        {
            var endpointActionData = endpoint.Metadata.GetRequiredMetadata<ControllerActionDescriptor>();

            var appRolesAttribute = endpointActionData.MethodInfo.GetCustomAttribute<RequireAppRolesAttribute>();

            if (appRolesAttribute is not null && appRolesAttribute.AppRoles.Contains(appRole) == false)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Restricted by role");
                return;
            }
        }
        
        await _next(context);
    }
}