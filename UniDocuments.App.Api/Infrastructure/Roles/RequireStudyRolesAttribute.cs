using UniDocuments.App.Shared.Users.Enums;

namespace UniDocuments.App.Api.Infrastructure.Roles;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireStudyRolesAttribute : Attribute
{
    public StudyRole[] StudyRoles { get; }

    public RequireStudyRolesAttribute(params StudyRole[] studyRoles)
    {
        StudyRoles = studyRoles;
    }
}