using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Api.Infrastructure.Roles;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireAppRolesAttribute : Attribute
{
    public AppRole[] AppRoles { get; }

    public RequireAppRolesAttribute(params AppRole[] appRoles)
    {
        AppRoles = appRoles;
    }
}