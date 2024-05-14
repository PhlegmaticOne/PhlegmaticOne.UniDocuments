using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using UniDocuments.App.Application.Infrastructure.Extensions;

namespace UniDocuments.App.Api.Infrastructure.Roles;

public class RequireStudyRolesMiddleware
{
    private readonly RequestDelegate _next;

    public RequireStudyRolesMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var studyRole = context.User.StudyRole();
        var endpoint = context.GetEndpoint();

        if (endpoint is not null)
        {
            var endpointActionData = context.GetEndpoint()!.Metadata.GetRequiredMetadata<ControllerActionDescriptor>();
            var studyRolesAttribute = endpointActionData.MethodInfo.GetCustomAttribute<RequireStudyRolesAttribute>();

            if (studyRolesAttribute is not null && studyRolesAttribute.StudyRoles.Contains(studyRole) == false)
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